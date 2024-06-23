using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Remoting.Messaging;

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
        private static readonly Regex userdataRx = new Regex("^(.+?)/(.+?)/(.+)$");
        private static readonly Regex recvMoneyRx = new Regex("^([a-z_]+) pay " + META_DELIM + " (\\d+)$");

        private static IrcClient client = new IrcClient();
        public static Dictionary<string, Userdata> userData = new Dictionary<string, Userdata>();
        private static DateTime lastDeath = new DateTime();
        private static DateTime lastPay = new DateTime();
        private static DateTime lastMessage = new DateTime();
        private static bool lastStatus = false;
        public static string lastName, lastChannel, prevChannel, lastQuery, lastFaction, nickBeforeRecover;
        private static bool retry = false;

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
        public static void ShowInformation(string message)
        {
            CRCDisplay.AddInformation(message);
            CRCGame.AddInformation(message);
        }

        public static void NotifyAndLogin()
        {
            ShowInformation(String.Format(CRCStrings.Localize("try_identify_as"), nickBeforeRecover));
            client.SendMessage(SendType.Message, "NickServ", "IDENTIFY " + CRCOptions.Password);
            CRCOptions.Name = nickBeforeRecover;
         
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
            if (CRCOptions.GetFaction() != lastFaction)
            {
                if (userData.ContainsKey(CRCOptions.Name))
                    userData[CRCOptions.Name].Faction = CRCOptions.GetFaction();
                client.SendMessage(SendType.CtcpReply, CRCOptions.ChannelProxy(), "AMOGUS " + UserDataUpdate());
                lastFaction = CRCOptions.GetFaction();
                UpdateUsers();
            }
        }

        public static void OnChannelSwitch()
        {
            if (CRCOptions.ChannelProxy() != lastChannel)
            {
                userData.Clear();
                client.RfcPart(lastChannel);
                client.RfcJoin(CRCOptions.ChannelProxy());
                lastChannel = CRCOptions.ChannelProxy();
            }
        }

        public static void UpdateStatus()
        {
            if (CRCGame.IsInGame != lastStatus)
            {
                if (userData.ContainsKey(CRCOptions.Name))
                    userData[CRCOptions.Name].IsInGame = CRCGame.IsInGame.ToString();
                client.SendMessage(SendType.CtcpReply, CRCOptions.ChannelProxy(), "AMOGUS " + UserDataUpdate());
                lastStatus = CRCGame.IsInGame;
                UpdateUsers();
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
            if ((DateTime.Now - lastMessage).TotalSeconds < 5)
            {
                ShowError(CRCStrings.Localize("crc_message_cooldown"));
                return;
            }
            else
            {
                client.SendMessage(SendType.Message, CRCOptions.ChannelProxy(), message);
                CRCDisplay.OnOwnChannelMessage(CRCOptions.Name, message);
                CRCGame.OnChannelMessage(CRCOptions.Name, CRCOptions.GetFaction(), message);
                lastMessage = DateTime.Now;
            }
        }

        public static void SendDeath(string message)
        {
            string nick = CRCStrings.RandomName(CRCOptions.GameFaction);
            client.SendMessage(SendType.Message, CRCOptions.ChannelProxy(), nick + FAKE_DELIM + CRCOptions.GetFaction() + META_DELIM + message);
            ShowChannelMessage(nick, CRCOptions.GameFaction, message);
        }

        public static void SendQuery(string nick, string message)
        {
            if (CRCOptions.blockListData.ContainsKey(nick))
            {
                CRCClient.ShowError(String.Format(CRCStrings.Localize("user_is_blocked"), nick));
                return;
            }

            if (nick.Equals("NickServ")) {
                client.SendMessage(SendType.Message, nick, message);
            }
            else
            {
                client.SendMessage(SendType.Message, nick, CRCOptions.GetFaction() + META_DELIM + message);
            }
            ShowQueryMessage(CRCOptions.Name, nick, CRCOptions.GameFaction, message);
        }

        private static void ShowQueryMessage(string from, string to, string faction, string message)
        {
            CRCDisplay.OnQueryMessage(from, to, message);
            CRCGame.OnQueryMessage(from, to, CRCOptions.GetFaction(), message);
        }

        public static void SendMoney(string nick, string message)
        {
            if (CRCOptions.blockListData.ContainsKey(nick))
            {
                CRCClient.ShowError(String.Format(CRCStrings.Localize("user_is_blocked"), nick));
                return;
            }
            if (CRCOptions.BlockPayments)
            {
                ShowError(CRCStrings.Localize("payment_blocked"));
                return;
            }

            int amount;
            bool acceptable = int.TryParse(message, out amount);
            if ((DateTime.Now - lastPay).TotalSeconds < 120)
            {
                ShowError(CRCStrings.Localize("crc_money_cooldown"));
                return;
            }    
            if (acceptable && amount <=1000000)
            {
                if (amount > CRCGame.ActorMoney)
                {
                    ShowError(CRCStrings.Localize("crc_money_none"));
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
                ShowError(CRCStrings.Localize("crc_money_toohigh"));
                return;
            }
        }

        public static bool SendReply(string message)
        {
            if (lastQuery != null)
            {
                if (CRCOptions.blockListData.ContainsKey(lastQuery))
                {
                    CRCClient.ShowError(String.Format(CRCStrings.Localize("user_is_blocked"), lastQuery));
                    return false;
                }
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
                faction = "actor_anonymous";
                return message;
            }
        }


        private static void OnRawMessage(object sender, IrcEventArgs e)
        {
            try
            {
                if (e.Data.Nick.Equals("NickServ"))
                {
                    HandleNickServMessage(e.Data.Message);
                }
            }
            catch {
                // ignore
            }
#if DEBUG
            debug?.AddRaw(e.Data.RawMessage);
#endif
        }

        private static void HandleNickServMessage(string message)
        {
            System.Console.WriteLine(message);
            if (message.Equals("Password accepted -- you are now recognized."))
            {
                HandleLoggedIn();
            }
            else if (message.Equals("Services' hold on your nickname has been released."))
            {
                NotifyAndLogin();
            }
            else if (message.StartsWith("This nickname is registered and protected."))
            {
                // error handler will handle this, we just don't want to duplicate messages
            }
            else if (message.StartsWith("Password incorrect."))
            {
                HandleIncorrectPassword();
            }
            else if (message.StartsWith("Your nickname isn't registered."))
            {
                if (!CRCOptions.DisableUnregisteredMessage) {
                    ShowInformation(CRCStrings.Localize("unregistered_nickname"));
                }
            }
            else
            {
                ShowHighlightMessage("NickServ", CRCOptions.GameFaction, message);
            }
        }

        private static void HandleIncorrectPassword()
        {
            ShowError(String.Format(CRCStrings.Localize("incorrect_password"), CRCOptions.Name));
            userData.Remove(CRCOptions.Name);
            lastName = nickBeforeRecover =  CRCOptions.Name = CRCOptions.Name + "_";
            client.RfcNick(nickBeforeRecover);
        }

        private static void HandleLoggedIn()
        {
            ShowInformation(String.Format(CRCStrings.Localize("logged_in_as"), nickBeforeRecover));
            CRCOptions.Name = nickBeforeRecover;
            lastName = nickBeforeRecover;
        }

        private static void OnCtcpRequest(object sender, CtcpEventArgs e)
        {
            if (CRCOptions.isHostBlocked(e))
            {
                return;
            }
            string from = e.Data.Nick;
            System.Console.WriteLine(from + " CTCP: " + e.CtcpCommand.ToUpper());
            switch (e.CtcpCommand.ToUpper())
            {
                case "USERDATA":
                    client.SendMessage(SendType.CtcpReply, CRCOptions.ChannelProxy(), "AMOGUS " + UserDataUpdate());
                    break;
                case "CLIENTINFO":
                    client.SendMessage(SendType.CtcpReply, from, "CLIENTINFO Supported CTCP commands: CLIENTINFO USERDATA PING VERSION");
                    break;
                case "PING":
                    client.SendMessage(SendType.CtcpReply, from, "PONG " + e.CtcpParameter);
                    break;
                case "VERSION":
                    client.SendMessage(SendType.CtcpReply, from, "VERSION Chernobyl Relay Chat Rebirth " + Application.ProductVersion);
                    break;
            }
        }

        private static void OnCtcpReply(object sender, CtcpEventArgs e)
        {
            string from = e.Data.Nick;
            if (CRCOptions.isHostBlocked(e))
            {
                return;
            }
            switch (e.CtcpCommand.ToUpper())
            {
                case "AMOGUS":
                    try
                    {
                        HandleCtcpAmogus(e);
                    }
                    catch {
                        // value probably malformed, skip silently 
                    }
                    break;
            }
        }

        private static void HandleCtcpAmogus(CtcpEventArgs e)
        {
            Match userdataMatch = userdataRx.Match(e.CtcpParameter);
            if (userdataMatch.Success)
            {

                userData[userdataMatch.Groups[1].Value].User = userdataMatch.Groups[1].Value;
            }

            userData[userdataMatch.Groups[1].Value].Faction = CRCStrings.ValidateFaction(userdataMatch.Groups[2].Value);
            userData[userdataMatch.Groups[1].Value].IsInGame = userdataMatch.Groups[3].Value;
#if DEBUG
            var debug = JsonConvert.SerializeObject(userData);
            System.Diagnostics.Debug.WriteLine(debug);
            System.Diagnostics.Debug.WriteLine(userdataMatch.Groups[1].Value + " " + userdataMatch.Groups[2].Value + " " + userdataMatch.Groups[3].Value);
#endif
            UpdateUsers();
        }

        private static void OnConnected(object sender, EventArgs e)
        {
            userData.Clear();
            nickBeforeRecover = lastName = CRCOptions.Name;
            lastChannel = CRCOptions.ChannelProxy();
            lastFaction = CRCOptions.GetFaction();
            client.Login(CRCOptions.Name, CRCStrings.Localize("crc_name") + " " + Application.ProductVersion);
            client.RfcJoin(CRCOptions.ChannelProxy());
            if (CRCOptions.Password.Length > 0)
            {
                NotifyAndLogin();
            }
            ShowInformation(CRCStrings.Localize("welcome_msg"));
        }

        private static void OnChannelActiveSynced(object sender, IrcEventArgs e)
        {
            if (CRCOptions.isHostBlocked(e))
            {
                return;
            }
            try
            {
                userData.Add(CRCOptions.Name, new Userdata { User = CRCOptions.Name, Faction = CRCOptions.GetFaction(), IsInGame = CRCGame.IsInGame.ToString() });
            }
            catch {
                System.Console.WriteLine("Could not add new userData for " + CRCOptions.Name);
            }
            foreach (ChannelUser user in client.GetChannel(e.Data.Channel).Users.Values)
            {
                if (!userData.ContainsKey(user.Nick))
                    userData.Add(user.Nick, new Userdata { User = user.Nick, Faction = "actor_anonymous", IsInGame = "False" });
            }
            client.SendMessage(SendType.CtcpRequest, e.Data.Channel, "USERDATA");
            client.SendMessage(SendType.CtcpReply, e.Data.Channel, "AMOGUS " + UserDataUpdate());
            prevChannel = CRCOptions.Channel;
            UpdateUsers();
        }

        private static void OnDisconnected(object sender, EventArgs e)
        {
            if (retry)
            {
                ShowInformation(CRCStrings.Localize("client_reconnecting"));
                client.Connect(CRCOptions.Server, 6667);
            }
        }

        private static void OnTopic(object sender, TopicEventArgs e)
        {
            ShowInformation(CRCStrings.Localize("client_topic") + e.Topic);
        }

        private static void OnTopicChange(object sender, TopicChangeEventArgs e)
        {
            ShowInformation(CRCStrings.Localize("client_topic_change") + e.NewTopic);
        }

        public static void OnSignalLost(string reason)
        {
            userData.Clear();
            client.RfcPart(CRCOptions.ChannelProxy(), reason);
        }

        public static void OnSignalRestored()
        {
            lastName = CRCOptions.Name;
            prevChannel = lastChannel;
            lastChannel = CRCOptions.ChannelProxy();
            lastFaction = CRCOptions.GetFaction();
            client.RfcJoin(CRCOptions.ChannelProxy());
        }

        private static void OnChannelMessage(object sender, IrcEventArgs e)
        {
            if (CRCOptions.isHostBlocked(e))
            {
                return;
            }
            string fakeNick, faction;
            string message = GetMetadata(e.Data.Message, out fakeNick, out faction);
            // If some cheeky m8 just sends delimiters, ignore it
            if (message.Length > 0)
            {
                string nick;
                if (fakeNick == null)
                {
                    nick = e.Data.Nick;
                    if (userData.ContainsKey(nick) && CRCOptions.GetFaction() == "actor_isg")
                    {
                        faction = userData[nick].Faction;
                    }
                    else if (userData.ContainsKey(nick) && userData[nick].Faction == "actor_isg" && CRCOptions.GetFaction() != "actor_isg")
                    {
                        faction = "actor_anonymous";
                    }
                    else if (userData.ContainsKey(nick) && userData[nick].Faction != "actor_isg")
                    {
                        faction = userData[nick].Faction;
                    }
                    else
                    {
                        faction = "actor_anonymous";
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
                    if (CRCOptions.SoundNotifications)
                        SystemSounds.Asterisk.Play();
                    ShowHighlightMessage(nick, faction, message);
                }
                else
                {
                    ShowChannelMessage(nick, faction, message);
                }
            }
        }

        private static void ShowHighlightMessage(string nick, string faction, string message)
        {
            CRCDisplay.OnHighlightMessage(nick, message, faction);
            CRCGame.OnHighlightMessage(nick, faction, message);
        }

        private static void ShowChannelMessage(string nick, string faction, string message)
        {
            CRCDisplay.OnChannelMessage(nick, message, faction);
            CRCGame.OnChannelMessage(nick, faction, message);
        }

        private static void OnQueryMessage(object sender, IrcEventArgs e)
        {
            if (CRCOptions.isHostBlocked(e))
            {
                return;
            }
            string raw = e.Data.Message;
            Match recvMoney = recvMoneyRx.Match(raw);

            // Metadata should not be used in queries, just throw it out
            string fakeNick, faction;
            string message = GetMetadata(e.Data.Message, out fakeNick, out faction);
            string nick = e.Data.Nick;
            if (recvMoney.Success)
            {
                if (CRCOptions.BlockPayments)
                {
                    return;
                }
                CRCDisplay.OnMoneyRecv(nick, message);
                CRCGame.OnMoneyRecv(nick, message);
            }
            // we should only show messages that have content
            else if (message.Length > 0)
            {
                lastQuery = e.Data.Nick;
                // Metadata should not be used in queries, just throw it out
                
                if (userData.ContainsKey(nick) && CRCOptions.GetFaction() == "actor_isg")
                {
                    faction = userData[nick].Faction;
                }
                else if (userData.ContainsKey(nick) && userData[nick].Faction == "actor_isg" && CRCOptions.GetFaction() != "actor_isg")
                {
                    faction = "actor_anonymous";
                }
                else if (userData.ContainsKey(nick) && userData[nick].Faction != "actor_isg")
                {
                    faction = userData[nick].Faction;
                }
                else
                {
                    faction = "actor_stalker";
                }
                System.Console.WriteLine(String.Format("{0}({2}) -> {1}: '{3}'", nick, CRCOptions.Name, faction, message));
                ShowQueryMessage(nick, CRCOptions.Name, faction, message);
            }
        }

        private static void OnJoin(object sender, JoinEventArgs e)
        {
            if (e.Who != client.Nickname)
            {
                if (CRCOptions.isHostBlocked(e))
                {
                    return;
                }
                userData.Add(e.Who, new Userdata { User = e.Who, Faction = "actor_anonymous", IsInGame = "False" });
                UpdateUsers();
                ShowInformation(e.Who + CRCStrings.Localize("client_join"));
            }
            else
            {
                CRCOptions.Name = e.Who;
                ShowInformation(CRCStrings.Localize("client_connected") + ChannelToChannelName(CRCOptions.Channel));
                CRCDisplay.OnConnected();
            }
        }

        private static void OnPart(object sender, PartEventArgs e)
        {
            if (e.Who == CRCOptions.Name)
            {
                ShowInformation(CRCStrings.Localize("client_own_part") + ChannelToChannelName(prevChannel));
                return;
            }
            if (CRCOptions.isHostBlocked(e))
            {
                return;
            }
            userData.Remove(e.Who);
            UpdateUsers();
            if (e.PartMessage != null)
            {
                if (e.PartMessage == "Underground")
                {
                    ShowInformation(String.Format(CRCStrings.Localize("clinet_part_underground"), e.Who));
                    return;
                }
                else if (e.PartMessage == "Surge")
                {
                    ShowInformation(String.Format(CRCStrings.Localize("clinet_part_surge"), e.Who));
                    return;
                }
            }
            ShowInformation(e.Who + CRCStrings.Localize("client_part"));
        }

        private static void OnQuit(object sender, QuitEventArgs e)
        {
            userData.Remove(e.Who);
            UpdateUsers();
            if (CRCOptions.isHostBlocked(e))
            {
                return;
            }
            ShowInformation(e.Who + CRCStrings.Localize("client_quit"));
        }

        private static void OnKick(object sender, KickEventArgs e)
        {
            string victim = e.Whom;
            if (victim == CRCOptions.Name)
            {
                userData.Clear();
                ShowError(CRCStrings.Localize("client_got_kicked") + e.KickReason);
                CRCDisplay.OnGotKicked();
            }
            else if (CRCOptions.isHostBlocked(e))
            {
                return;
            }
            else
            {
                userData.Remove(victim);
                ShowInformation(victim + CRCStrings.Localize("client_kicked") + e.KickReason);
            }
            UpdateUsers();
        }

        private static void OnNickChange(object sender, NickChangeEventArgs e)
        {
            string oldNick = e.OldNickname;
            string newNick = e.NewNickname;
#if DEBUG
            System.Console.WriteLine("Nick change: '{0}' -> '{1}'", oldNick, newNick);
#endif
            if (CRCOptions.isHostBlocked(e))
            {
                CRCOptions.addToBlockList(e);
                try
                {
                    userData.Remove(oldNick);
                    UpdateUsers();
                } catch
                {

                }
                return;
            }

            try
            {
                userData.Add(newNick, new Userdata { User = newNick, Faction = "actor_anonymous", IsInGame = "False" });
                userData[newNick] = userData[oldNick];
                userData.Remove(oldNick);
            }
            catch {
                if (String.IsNullOrEmpty(CRCOptions.Password)) {
                    CRCOptions.Name = lastName;
                }
                return;
            }
            if (newNick != client.Nickname)
            {
                ShowInformation(oldNick + CRCStrings.Localize("client_nick_change") + newNick);
            }
            else
            {
                CRCOptions.Name = newNick;
                ShowInformation(CRCStrings.Localize("client_own_nick_change") + newNick);
                if (CRCOptions.Password.Length > 0)
                {
                    NotifyAndLogin();
                }
            }
            UpdateUsers();
        }

        public static void UpdateUsers()
        {
            CRCDisplay.UpdateUsers();
            CRCGame.UpdateUsers();
        }

        private static void OnErrorMessage(object sender, IrcEventArgs e)
        {
            string message;
            switch (e.Data.ReplyCode)
            {
                case ReplyCode.ErrorBannedFromChannel:
                    message = CRCStrings.Localize("client_banned");
                    ShowError(message);
                    break;
                // What's the difference?
                case ReplyCode.ErrorNicknameInUse:
                case ReplyCode.ErrorNicknameCollision:
                    message = CRCStrings.Localize("client_nick_collision");
                    ShowError(message);
                    if (CRCOptions.Password.Length > 0) {
                        lastName = CRCOptions.Name;
                        string parameters = String.Format("{0} {1}", nickBeforeRecover, CRCOptions.Password);
                        ShowInformation(String.Format(CRCStrings.Localize("recover_nick"), nickBeforeRecover));
                        client.SendMessage(SendType.Message, "NickServ", String.Format("RECOVER {0}", parameters));
                        client.SendMessage(SendType.Message, "NickServ", String.Format("RELEASE {0}", parameters));
                        client.RfcNick(nickBeforeRecover);
                    }
                    break;
                // Don't care
                case ReplyCode.ErrorNoMotd:
                case ReplyCode.ErrorNotRegistered:
                    System.Console.WriteLine("Not registered or no motd. " + e.Data.Message);
                    break;
                case ReplyCode.ErrorNoSuchNickname:
                    ShowError(CRCStrings.Localize("error_no_such_nickname"));
                    break;
                default:
                    ShowError(e.Data.Message);
                    break;
            }
        }

        public static void ShowError(string message)
        {
            CRCDisplay.AddError(message);
            CRCGame.AddError(message);
        }

        private static string ChannelToChannelName(string rawChannel)
        {
            string Channel = "UNKNOWN";
            if (rawChannel == "#crcr_english") 
                Channel = "Main Channel (Eng)";
            if (rawChannel == "#crcr_english_rp") 
                Channel = "Roleplay Channel (Eng)";
            if (rawChannel == "#crcr_english_shitposting") 
                Channel = "Unmoderated Channel (Eng)";
            if (rawChannel == "#crcr_russian") 
                Channel = "Основной Канал (Русский)";
            if (rawChannel == "#crcr_russian_rp") 
                Channel = "Ролевой Канал (Русский)";
            if (rawChannel == "#crcr_tech_support") 
                Channel = "Tech Support/Техподдержка";
            return Channel;
        }

        private static string UserDataUpdate()
        {
            string userDataUpdate = CRCOptions.Name + "/" + CRCOptions.GetFaction() + "/" + CRCGame.IsInGame.ToString();
            return userDataUpdate;
        }

        internal static void askAboutNames()
        {
            client.RfcNames(CRCOptions.Channel);
        }
    }

    public class Userdata
    {
        public string User { get; set; }
        public string Faction { get; set; }
        public string IsInGame { get; set; }
    }
}
