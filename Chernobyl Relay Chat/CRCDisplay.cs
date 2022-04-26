using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    class CRCDisplay
    {
        private static ClientDisplay clientDisplay;

        public static void Start()
        {
            clientDisplay = new ClientDisplay();
            Application.Run(clientDisplay);
        }

        public static void Stop()
        {
            clientDisplay?.Invoke(new Action(() =>
                clientDisplay.Close())
                );
        }

        public static void AddInformation(string message)
        {
            clientDisplay?.AddInformation(message);
        }

        public static void AddError(string message)
        {
            clientDisplay?.AddError(message);
        }



        public static void OnConnected()
        {
            clientDisplay?.Enable();
        }

        public static void UpdateUsers()
        {
            clientDisplay?.UpdateUsers(CRCClient.userData);
        }

        public static void OnHighlightMessage(string nick, string message)
        {
            clientDisplay?.AddHighlightMessage(nick, message);
        }

        public static void OnChannelMessage(string nick, string message)
        {
            clientDisplay?.AddMessage(nick, message, Color.Black);
        }

        public static void OnOwnChannelMessage(string nick, string message)
        {
            clientDisplay?.AddMessage(nick, message, Color.Gray);
        }

        public static void OnQueryMessage(string from, string to, string message)
        {
            if (CRCOptions.SoundNotifications)
                SystemSounds.Asterisk.Play();
            clientDisplay?.AddMessage(from + " -> " + to, message, Color.DeepPink);
        }

        public static void OnMoneySent(string from, string to, string message)
        {
            if (CRCOptions.SoundNotifications)
                SystemSounds.Asterisk.Play();
            clientDisplay?.AddMessage(from + CRCStrings.Localize("crc_money") + to, message + " RUB", Color.DarkBlue);
        }

        public static void OnMoneyRecv(string from, string message)
        {
            if (CRCOptions.SoundNotifications)
                SystemSounds.Asterisk.Play();
            clientDisplay?.AddMessage(from + CRCStrings.Localize("crc_money_recv"), message + " RUB!", Color.DarkBlue);
        }

        public static void OnGotKicked()
        {
            clientDisplay?.Disable();
        }

        public static void OnChannelUpdateFromGame(int index)
        {
            clientDisplay?.OnChannelUpdateFromGame(index);
        }
    }
}
