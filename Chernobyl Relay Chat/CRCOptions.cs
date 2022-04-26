using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security;

namespace Chernobyl_Relay_Chat
{
    class CRCOptions
    {
#if DEBUG
        private static RegistryKey registry = Registry.CurrentUser.CreateSubKey(@"Software\Chernobyl Relay Chat Rebirth Debug");
#else
        private static RegistryKey registry = Registry.CurrentUser.CreateSubKey(@"Software\Chernobyl Relay Chat Rebirth");
#endif

        public const string Server = "irc.slashnet.org";
        public const string InPath = @"\..\gamedata\configs\crc_input.txt";
        public const string OutPath = @"\..\gamedata\configs\crc_output.txt";

        public static string Language = "eng";
        public static string Channel;
        public static Point DisplayLocation;
        public static Size DisplaySize;

        public static bool AutoFaction;
        public static string GameFaction;
        public static string ManualFaction;
        public static string Name;
        public static bool SendDeath;
        public static bool ReceiveDeath;
        public static int DeathInterval;
        public static bool ShowTimestamps;
        public static bool SoundNotifications;

        public static int NewsDuration;
        public static string ChatKey;
        public static bool NewsSound;
        public static bool CloseChat;

        private static readonly Dictionary<string, string> defaultChannel = new Dictionary<string, string>()
        {
            ["eng"] = "#crcr_english",
            ["rus"] = "#crcr_russian",
        };

        public static string ChannelProxy()
        {
#if DEBUG
            return Channel + "_debug";
#else
            return Channel;
#endif
        }

        public static string GetFaction()
        {
            if (AutoFaction)
                return GameFaction;
            else
                return ManualFaction;
        }

        public static bool Load()
        {
            try
            {
                Language = (string)registry.GetValue("Language", null);
                Channel = (string)registry.GetValue("Channel", null);
                if (Language == null)
                {
                    using (LanguagePrompt languagePrompt = new LanguagePrompt())
                    {
                        languagePrompt.ShowDialog();
                        Language = languagePrompt.Result;
                    }
                }
                if (Channel == null)
                {
                    Channel = defaultChannel[Language];
                }

                DisplayLocation = new Point((int)registry.GetValue("DisplayLocationX", 0),
                    (int)registry.GetValue("DisplayLocationY", 0));
                DisplaySize = new Size((int)registry.GetValue("DisplayWidth", 0),
                    (int)registry.GetValue("DisplayHeight", 0));

                AutoFaction = Convert.ToBoolean((string)registry.GetValue("AutoFaction", "True"));
                GameFaction = (string)registry.GetValue("GameFaction", "actor_stalker");
                ManualFaction = (string)registry.GetValue("ManualFaction", "actor_stalker");
                Name = (string)registry.GetValue("Name", CRCStrings.RandomIrcName(GetFaction()));
                SendDeath = Convert.ToBoolean((string)registry.GetValue("SendDeath", "True"));
                ReceiveDeath = Convert.ToBoolean((string)registry.GetValue("ReceiveDeath", "True"));
                DeathInterval = (int)registry.GetValue("DeathInterval", 0);
                ShowTimestamps = Convert.ToBoolean((string)registry.GetValue("ShowTimestamps", "True"));
                SoundNotifications = Convert.ToBoolean((string)registry.GetValue("SoundNotifications", "True"));

                NewsDuration = (int)registry.GetValue("NewsDuration", 10);
                ChatKey = (string)registry.GetValue("ChatKey", "RETURN");
                NewsSound = Convert.ToBoolean((string)registry.GetValue("NewsSound", "True"));
                CloseChat = Convert.ToBoolean((string)registry.GetValue("CloseChat", "True"));

                Save();
                return true;
            }
            catch (Exception ex) when (ex is SecurityException || ex is UnauthorizedAccessException)
            {
                return false;
            }
        }

        public static void Save()
        {
            registry.SetValue("Language", Language);
            registry.SetValue("Channel", Channel);
            registry.SetValue("DisplayLocationX", DisplayLocation.X);
            registry.SetValue("DisplayLocationY", DisplayLocation.Y);
            registry.SetValue("DisplayWidth", DisplaySize.Width);
            registry.SetValue("DisplayHeight", DisplaySize.Height);

            registry.SetValue("AutoFaction", AutoFaction);
            registry.SetValue("GameFaction", GameFaction);
            registry.SetValue("ManualFaction", ManualFaction);
            registry.SetValue("Name", Name);
            registry.SetValue("SendDeath", SendDeath);
            registry.SetValue("ReceiveDeath", ReceiveDeath);
            registry.SetValue("DeathInterval", DeathInterval);
            registry.SetValue("ShowTimestamps", ShowTimestamps);
            registry.SetValue("SoundNotifications", SoundNotifications);

            registry.SetValue("NewsDuration", NewsDuration);
            registry.SetValue("ChatKey", ChatKey);
            registry.SetValue("NewsSound", NewsSound);
            registry.SetValue("CloseChat", CloseChat);
        }
    }
}
