using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
#if DEBUG
using System.Threading;
#endif

namespace Chernobyl_Relay_Chat
{
    public class CRCClient
    {
        private const char META_DELIM = '☺'; // Separates metadata
        private const char FAKE_DELIM = '☻'; // Separates fake nick for death messages
        private static readonly Regex metaRx = new Regex("^(.*?)" + META_DELIM + "(.*)$");
        private static readonly Regex deathRx = new Regex("^(.*?)" + FAKE_DELIM + "(.*)$");
        private static readonly Regex commandArgsRx = new Regex(@"\S+");

        private static IrcClient client = new IrcClient();
        private static Dictionary<string, string> crcNicks = new Dictionary<string, string>();
        private static DateTime lastDeath = new DateTime();
        private static string lastName, lastChannel, lastQuery, lastFaction;
        private static bool retry = false;

        public static List<string> Users = new List<string>();

#if DEBUG
        private static DebugDisplay debug = new DebugDisplay();
        private static Thread debugThread;
#endif

        public static void Start()
        {
#if DEBUG
            debugThread = new Thread(() => Application.Run(debug));
            debugThread.Start();
#endif
            client.Encoding = Encoding.UTF8;
            client.SendDelay = 200;
            client.ActiveChannelSyncing = true;

            client.OnConnected += new EventHandler(OnConnected);
            client.OnChannelActiveSynced += new IrcEventHandler(OnChannelActiveSynced);
            client.OnRawMessage += new IrcEventHandler(OnRawMessage);
            client.OnChannelMessage += new IrcEventHandler(OnChannelMessage);
            client.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
            client.OnJoin += new JoinEventHandler(OnJoin);
            client.OnPart += new PartEventHandler(OnPart);
            client.OnQuit += new QuitEventHandler(OnQuit);
            client.OnNickChange += new NickChangeEventHandler(OnNickChange);
            client.OnErrorMessage += new IrcEventHandler(OnErrorMessage);
            client.OnKick += new KickEventHandler(OnKick);
            client.OnDisconnected += new EventHandler(OnDisconnected);
            client.OnTopic += new TopicEventHandler(OnTopic);
            client.OnTopicChange += new TopicChangeEventHandler(OnTopicChange);
            client.OnCtcpRequest += new CtcpEventHandler(OnCtcpRequest);
            client.OnCtcpReply += new CtcpEventHandler(OnCtcpReply);

            try
            {
                client.Connect(CRCOptions.Server, 6667);
                client.Listen();
            }
            catch (CouldNotConnectException)
            {
                MessageBox.Show(CRCStrings.Localize("client_connection_error"), CRCStrings.Localize("crc_name"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                CRCDisplay.Stop();
            }
#if DEBUG
            debug.Invoke(new Action(() => debug.Close()));
            debugThread.Join();
#endif
        }

        public static void Stop()
        {
            if (client.IsConnected)
            {
                client.RfcQuit("Safe");
            }
        }

        public static void UpdateSettings()
        {
            if (CRCOptions.Name != lastName)
            {
                client.RfcNick(CRCOptions.Name);
                lastName = CRCOptions.Name;
            }
            if (CRCOptions.ChannelProxy() != lastChannel)
            {
                Users.Clear();
                client.RfcPart(lastChannel);
                client.RfcJoin(CRCOptions.ChannelProxy());
                lastChannel = CRCOptions.ChannelProxy();
            }
            if (CRCOptions.GetFaction() != lastFaction)
            {
                foreach (string nick in crcNicks.Keys)
                {
                    client.SendMessage(SendType.CtcpReply, nick, CRCOptions.GetFaction());
                }
                lastFaction = CRCOptions.GetFaction();
            }
        }

        public static void ChangeNick(string nick)
        {
            CRCOptions.Name = nick;
            lastName = nick;
            client.RfcNick(nick);
        }

        public static void Send(string message)
        {
            client.SendMessage(SendType.Message, CRCOptions.ChannelProxy(), message);
            CRCDisplay.OnOwnChannelMessage(CRCOptions.Name, message);
            CRCGame.OnChannelMessage(CRCOptions.Name, CRCOptions.GetFaction(), message);
        }

        public static void SendDeath(string message)
        {
            string nick = CRCStrings.RandomName(CRCOptions.GameFaction);
            client.SendMessage(SendType.Message, CRCOptions.ChannelProxy(), nick + FAKE_DELIM + CRCOptions.GetFaction() + META_DELIM + message);
            CRCDisplay.OnChannelMessage(nick, message);
            CRCGame.OnChannelMessage(nick, CRCOptions.GameFaction, message);
        }

        public static void SendQuery(string nick, string message)
        {
            client.SendMessage(SendType.Message, nick, CRCOptions.GetFaction() + META_DELIM + message);
            CRCDisplay.OnQueryMessage(CRCOptions.Name, nick, message);
            CRCGame.OnQueryMessage(CRCOptions.Name, nick, CRCOptions.GetFaction(), message);
        }

        public static bool SendReply(string message)
        {
            if (lastQuery != null)
            {
                SendQuery(lastQuery, message);
                return true;
            }
            return false;
        }

        private static string GetMetadata(string message, out string fakeNick, out string faction)
        {
            Match metaMatch = metaRx.Match(message);
            if (metaMatch.Success)
            {
                Match deathMatch = deathRx.Match(metaMatch.Groups[1].Value);
                if (deathMatch.Success)
                {
                    fakeNick = deathMatch.Groups[1].Value;
                    faction = CRCStrings.ValidateFaction(deathMatch.Groups[2].Value);
                    return metaMatch.Groups[2].Value;
                }
                else
                {
                    fakeNick = null;
                    faction = CRCStrings.ValidateFaction(metaMatch.Groups[1].Value);
                    return metaMatch.Groups[2].Value;
                }
            }
            else
            {
                fakeNick = null;
                faction = "actor_stalker";
                return message;
            }
        }



        private static void OnRawMessage(object sender, IrcEventArgs e)
        {
#if DEBUG
            debug?.AddRaw(e.Data.RawMessage);
#endif
        }

        private static void OnCtcpRequest(object sender, CtcpEventArgs e)
        {
            string from = e.Data.Nick;
            switch(e.CtcpCommand.ToUpper())
            {
                case "CLIENTINFO":
                    client.SendMessage(SendType.CtcpReply, from, "CLIENTINFO Supported CTCP commands: CLIENTINFO FACTION PING VERSION");
                    break;
                case "FACTION":
                    if(!crcNicks.ContainsKey(from))
                    {
                        crcNicks[from] = "actor_stalker";
                        client.SendMessage(SendType.CtcpRequest, from, "FACTION");
                    }
                    client.SendMessage(SendType.CtcpReply, from, "FACTION " + CRCOptions.GetFaction());
                    break;
                case "PING":
                    client.SendMessage(SendType.CtcpReply, from, "PING " + e.CtcpParameter);
                    break;
                case "VERSION":
                    client.SendMessage(SendType.CtcpReply, from, "VERSION Chernobyl Relay Chat Rebirth " + Application.ProductVersion);
                    break;
            }
        }

        private static void OnCtcpReply(object sender, CtcpEventArgs e)
        {
            string from = e.Data.Nick;
            switch(e.CtcpCommand.ToUpper())
            {
                case "CLIENTINFO":
                    if(e.CtcpParameter.Contains("FACTION"))
                    {
                        crcNicks[from] = "actor_stalker";
                        client.SendMessage(SendType.CtcpRequest, from, "FACTION");
                    }
                    break;
                case "FACTION":
                    crcNicks[from] = CRCStrings.ValidateFaction(e.CtcpParameter);
                    break;
            }
        }

        private static void OnConnected(object sender, EventArgs e)
        {
            Users.Clear();
            crcNicks.Clear();
            lastName = CRCOptions.Name;
            lastChannel = CRCOptions.ChannelProxy();
            lastFaction = CRCOptions.GetFaction();
            client.Login(CRCOptions.Name, CRCStrings.Localize("crc_name") + " " + Application.ProductVersion);
            client.RfcJoin(CRCOptions.ChannelProxy());
        }

        private static void OnChannelActiveSynced(object sender, IrcEventArgs e)
        {
            foreach (ChannelUser user in client.GetChannel(e.Data.Channel).Users.Values)
                Users.Add(user.Nick);
            Users.Sort();
            CRCDisplay.UpdateUsers();
            CRCGame.UpdateUsers();
            client.SendMessage(SendType.CtcpRequest, e.Data.Channel, "CLIENTINFO");
        }

        private static void OnDisconnected(object sender, EventArgs e)
        {
            if (retry)
            {
                string message = CRCStrings.Localize("client_reconnecting");
                CRCDisplay.AddInformation(message);
                CRCGame.AddInformation(message);
                client.Connect(CRCOptions.Server, 6667);
            }
        }

        private static void OnTopic(object sender, TopicEventArgs e)
        {
            string message = CRCStrings.Localize("client_topic") + e.Topic;
            CRCDisplay.AddInformation(message);
            CRCGame.AddInformation(message);
        }

        private static void OnTopicChange(object sender, TopicChangeEventArgs e)
        {
            string message = CRCStrings.Localize("client_topic_change") + e.NewTopic;
            CRCDisplay.AddInformation(message);
            CRCGame.AddInformation(message);
        }

        private static void OnChannelMessage(object sender, IrcEventArgs e)
        {
            string fakeNick, faction;
            string message = GetMetadata(e.Data.Message, out fakeNick, out faction);
            // If some cheeky m8 just sends delimiters, ignore it
            if (message.Length > 0)
            {
                string nick;
                if (fakeNick == null)
                {
                    nick = e.Data.Nick;
                    faction = crcNicks.ContainsKey(nick) ? crcNicks[nick] : "actor_stalker";
                }
                else if (CRCOptions.ReceiveDeath && (DateTime.Now - lastDeath).TotalSeconds > CRCOptions.DeathInterval)
                {
                    lastDeath = DateTime.Now;
                    nick = e.Data.Nick; //fakeNick;
                }
                else
                    return;
                if (message.Contains(CRCOptions.Name))
                {
                    SystemSounds.Asterisk.Play();
                    CRCDisplay.OnHighlightMessage(nick, message);
                    CRCGame.OnHighlightMessage(nick, faction, message);
                }
                else
                {
                    CRCDisplay.OnChannelMessage(nick, message);
                    CRCGame.OnChannelMessage(nick, faction, message);
                }
            }
        }

        private static void OnQueryMessage(object sender, IrcEventArgs e)
        {
            lastQuery = e.Data.Nick;
            // Metadata should not be used in queries, just throw it out
            string fakeNick, faction;
            string message = GetMetadata(e.Data.Message, out fakeNick, out faction);
            string nick = e.Data.Nick;
            faction = crcNicks.ContainsKey(nick) ? crcNicks[nick] : "actor_stalker";
            CRCDisplay.OnQueryMessage(nick, CRCOptions.Name, message);
            CRCGame.OnQueryMessage(nick, CRCOptions.Name, faction, message);
        }

        private static void OnJoin(object sender, JoinEventArgs e)
        {
            if (e.Who != client.Nickname)
            {
                Users.Add(e.Who);
                Users.Sort();
                CRCDisplay.UpdateUsers();
                CRCGame.UpdateUsers();
                string message = e.Who + CRCStrings.Localize("client_join");
                CRCDisplay.AddInformation(message);
                CRCGame.AddInformation(message);
            }
            else
            {
                CRCOptions.Name = e.Who;
                string message = CRCStrings.Localize("client_connected");
                CRCDisplay.AddInformation(message);
                CRCGame.AddInformation(message);
                CRCDisplay.OnConnected();
            }
        }

        private static void OnPart(object sender, PartEventArgs e)
        {
            if (e.Who != CRCOptions.Name)
            {
                crcNicks.Remove(e.Who);
                Users.Remove(e.Who);
                Users.Sort();
                CRCDisplay.UpdateUsers();
                CRCGame.UpdateUsers();
                string message = e.Who + CRCStrings.Localize("client_part");
                CRCDisplay.AddInformation(message);
                CRCGame.AddInformation(message);
            }
            else
            {
                string message = CRCStrings.Localize("client_own_part");
                CRCDisplay.AddInformation(message);
                CRCGame.AddInformation(message);
            }
        }

        private static void OnQuit(object sender, QuitEventArgs e)
        {
            crcNicks.Remove(e.Who);
            Users.Remove(e.Who);
            Users.Sort();
            CRCDisplay.UpdateUsers();
            CRCGame.UpdateUsers();
            string message = e.Who + CRCStrings.Localize("client_part");
            CRCDisplay.AddInformation(message);
            CRCGame.AddInformation(message);
        }

        private static void OnKick(object sender, KickEventArgs e)
        {
            string victim = e.Whom;
            if (victim == CRCOptions.Name)
            {
                Users.Clear();
                string message = CRCStrings.Localize("client_got_kicked") + e.KickReason;
                CRCDisplay.AddError(message);
                CRCGame.AddError(message);
                CRCDisplay.OnGotKicked();
            }
            else
            {
                crcNicks.Remove(e.Who);
                Users.Remove(victim);
                Users.Sort();
                string message = victim + CRCStrings.Localize("client_kicked") + e.KickReason;
                CRCDisplay.AddInformation(message);
                CRCGame.AddInformation(message);
            }
            CRCDisplay.UpdateUsers();
            CRCGame.UpdateUsers();
        }

        private static void OnNickChange(object sender, NickChangeEventArgs e)
        {
            string oldNick = e.OldNickname;
            string newNick = e.NewNickname;
            Users.Remove(oldNick);
            Users.Add(newNick);
            Users.Sort();
            CRCDisplay.UpdateUsers();
            CRCGame.UpdateUsers();
            if (newNick != client.Nickname)
            {
                if (crcNicks.ContainsKey(oldNick))
                {
                    crcNicks[newNick] = crcNicks[oldNick];
                    crcNicks.Remove(oldNick);
                }
                string message = oldNick + CRCStrings.Localize("client_nick_change") + newNick;
                CRCDisplay.AddInformation(message);
                CRCGame.AddInformation(message);
            }
            else
            {
                CRCOptions.Name = newNick;
                string message = CRCStrings.Localize("client_own_nick_change") + newNick;
                CRCDisplay.AddInformation(message);
                CRCGame.AddInformation(message);
            }
        }

        private static void OnErrorMessage(object sender, IrcEventArgs e)
        {
            string message;
            switch (e.Data.ReplyCode)
            {
                case ReplyCode.ErrorBannedFromChannel:
                    message = CRCStrings.Localize("client_banned");
                    CRCDisplay.AddError(message);
                    CRCGame.AddError(message);
                    break;
                // What's the difference?
                case ReplyCode.ErrorNicknameInUse:
                case ReplyCode.ErrorNicknameCollision:
                    message = CRCStrings.Localize("client_nick_collision");
                    CRCDisplay.AddError(message);
                    CRCGame.AddError(message);
                    break;
                // Don't care
                case ReplyCode.ErrorNoMotd:
                case ReplyCode.ErrorNotRegistered:
                    break;
                default:
                    CRCDisplay.AddError(e.Data.Message);
                    CRCGame.AddError(e.Data.Message);
                    break;
            }
        }
    }
}
