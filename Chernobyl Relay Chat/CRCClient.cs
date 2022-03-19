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
        public static Dictionary<string, string> InGameStatus = new Dictionary<string , string>();
        private static DateTime lastDeath = new DateTime();
        private static DateTime lastPay = new DateTime();
        private static bool lastStatus = false;
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

        public static void UpdateStatus()
        {
            if (CRCGame.IsInGame != lastStatus)
            {
                foreach (string nick in InGameStatus.Keys)
                {
                    client.SendMessage(SendType.CtcpReply, nick, "STATUS " + CRCGame.IsInGame.ToString());
                }
                lastStatus = CRCGame.IsInGame;
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

        public static void SendMoney(string nick, string message)
        {
            int amount;
            bool acceptable = int.TryParse(message, out amount);
            if ((DateTime.Now - lastPay).TotalSeconds < 120)
            {
                CRCGame.AddError(CRCStrings.Localize("crc_money_cooldown"));
                CRCDisplay.AddError(CRCStrings.Localize("crc_money_cooldown"));
                return;
            }    
            if (acceptable && amount <=1000000)
            {
                if (amount > CRCGame.ActorMoney)
                {
                    CRCGame.AddError(CRCStrings.Localize("crc_money_none"));
                    CRCDisplay.AddError(CRCStrings.Localize("crc_money_none"));
                    return;
                }
                else 
                {
                    client.SendMessage(SendType.Message, nick, CRCOptions.GetFaction() + " pay " + META_DELIM + amount.ToString());
                    CRCDisplay.OnMoneySent(CRCOptions.Name, nick, amount.ToString());
                    CRCGame.OnMoneySent(CRCOptions.Name, nick, CRCOptions.GetFaction(), amount.ToString());
                    lastPay = DateTime.Now;
                }
            }
            else
            {
                CRCGame.AddError(CRCStrings.Localize("crc_money_toohigh"));
                CRCDisplay.AddError(CRCStrings.Localize("crc_money_toohigh"));
                return;
            }
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
                    client.SendMessage(SendType.CtcpReply, from, "CLIENTINFO Supported CTCP commands: CLIENTINFO FACTION PING VERSION STATUS");
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
                case "STATUS":
                    if (!InGameStatus.ContainsKey(from))
                    {
                        InGameStatus[from] = "False";
                        client.SendMessage(SendType.CtcpRequest, from, "STATUS");
                    }
                    client.SendMessage(SendType.CtcpReply, from, "STATUS " + CRCGame.IsInGame.ToString());
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
                    if(e.CtcpParameter.Contains("STATUS"))
                    {
                        InGameStatus[from] = "False";
                        client.SendMessage(SendType.CtcpRequest, from, "STATUS");
                    }
                    break;
                case "FACTION":
                    crcNicks[from] = CRCStrings.ValidateFaction(e.CtcpParameter);
                    break;
                case "STATUS":
                    InGameStatus[from] = e.CtcpParameter;
                    CRCGame.UpdateUsers();
                    CRCDisplay.UpdateUsers();
                    break;
            }
        }

        private static void OnConnected(object sender, EventArgs e)
        {
            Users.Clear();
            crcNicks.Clear();
            InGameStatus.Clear();
            lastName = CRCOptions.Name;
            lastChannel = CRCOptions.ChannelProxy();
            lastFaction = CRCOptions.GetFaction();
            client.Login(CRCOptions.Name, CRCStrings.Localize("crc_name") + " " + Application.ProductVersion);
            client.RfcJoin(CRCOptions.ChannelProxy());
            InGameStatus[CRCOptions.Name] = CRCGame.IsInGame.ToString();
        }

        private static void OnChannelActiveSynced(object sender, IrcEventArgs e)
        {
            foreach (ChannelUser user in client.GetChannel(e.Data.Channel).Users.Values)
            {
                Users.Add(user.Nick);
                InGameStatus[user.Nick] = "False";
                client.SendMessage(SendType.CtcpRequest, user.Nick, "STATUS");
            }
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
                    //faction = crcNicks.ContainsKey(nick) ? crcNicks[nick] : "actor_stalker";
                    if (crcNicks.ContainsKey(nick) && CRCOptions.GetFaction() == "actor_isg")
                    {
                        faction = crcNicks[nick];
                    }
                    else if (crcNicks.ContainsKey(nick) && crcNicks[nick] == "actor_isg" && CRCOptions.GetFaction() != "actor_isg")
                    {
                        faction = "actor_anonymous";
                    }
                    else if (crcNicks.ContainsKey(nick) && crcNicks[nick] != "actor_isg")
                    {
                        faction = crcNicks[nick];
                    }
                    else
                    {
                        faction = "actor_stalker";
                    }
                }
                else if (CRCOptions.ReceiveDeath && (DateTime.Now - lastDeath).TotalSeconds > CRCOptions.DeathInterval)
                {
                    lastDeath = DateTime.Now;
                    nick = fakeNick; //e.Data.Nick;
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
            string raw = e.Data.Message;
            bool check = raw.Contains("pay");
            if (check)
            {
                // Metadata should not be used in queries, just throw it out
                string fakeNick, faction;
                string message = GetMetadata(e.Data.Message, out fakeNick, out faction);
                string nick = e.Data.Nick;
                //faction = crcNicks.ContainsKey(nick) ? crcNicks[nick] : "actor_stalker";
                CRCDisplay.OnMoneyRecv(nick, message);
                CRCGame.OnMoneyRecv(nick, message);
            }
            else
            {
                lastQuery = e.Data.Nick;
                // Metadata should not be used in queries, just throw it out
                string fakeNick, faction;
                string message = GetMetadata(e.Data.Message, out fakeNick, out faction);
                string nick = e.Data.Nick;
                //faction = crcNicks.ContainsKey(nick) ? crcNicks[nick] : "actor_stalker";
                if (crcNicks.ContainsKey(nick) && CRCOptions.GetFaction() == "actor_isg")
                {
                    faction = crcNicks[nick];
                }
                else if (crcNicks.ContainsKey(nick) && crcNicks[nick] == "actor_isg" && CRCOptions.GetFaction() != "actor_isg")
                {
                    faction = "actor_anonymous";
                }
                else if (crcNicks.ContainsKey(nick) && crcNicks[nick] != "actor_isg")
                {
                    faction = crcNicks[nick];
                }
                else
                {
                    faction = "actor_stalker";
                }
                CRCDisplay.OnQueryMessage(nick, CRCOptions.Name, message);
                CRCGame.OnQueryMessage(nick, CRCOptions.Name, faction, message);
            }
        }

        private static void OnJoin(object sender, JoinEventArgs e)
        {
            if (e.Who != client.Nickname)
            {
                Users.Add(e.Who);
                InGameStatus[e.Who] = "False";
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
                InGameStatus.Remove(e.Who);
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
            InGameStatus.Remove(e.Who);
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
                InGameStatus.Remove(e.Who);
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
            InGameStatus.Remove(oldNick);
            InGameStatus[newNick] = "False";
            client.SendMessage(SendType.CtcpRequest, newNick, "STATUS");
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
                if (InGameStatus.ContainsKey(oldNick))
                {
                    InGameStatus[newNick] = InGameStatus[oldNick];
                    InGameStatus.Remove(oldNick);
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
                case ReplyCode.ErrorNoSuchNickname:
                    message = CRCStrings.Localize("error_no_such_nickname");
                    CRCDisplay.AddError(message);
                    CRCGame.AddError(message);
                    break;
                default:
                    CRCDisplay.AddError(e.Data.Message);
                    CRCGame.AddError(e.Data.Message);
                    break;
            }
        }
    }
}
