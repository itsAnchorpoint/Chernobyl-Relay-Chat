using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Chernobyl_Relay_Chat
{
    public partial class ClientDisplay : Form, ICRCSendable
    {
        private Font mainFont, boldFont, timeFont;

        public ClientDisplay()
        {
            InitializeComponent();
            Text = CRCStrings.Localize("crc_name") + " " + Application.ProductVersion;
            buttonSend.Text = CRCStrings.Localize("display_send");
            buttonOptions.Text = CRCStrings.Localize("display_options");
        }

        private void ClientDisplay_Load(object sender, EventArgs e)
        {
            mainFont = richTextBoxMessages.Font;
            boldFont = new Font(mainFont, FontStyle.Bold);
            timeFont = new Font("Courier New", mainFont.SizeInPoints, FontStyle.Regular);
            if (CRCOptions.DisplaySize != new Size(0, 0))
            {
                Location = CRCOptions.DisplayLocation;
                Size = CRCOptions.DisplaySize;
            }

            AddInformation(CRCStrings.Localize("display_connecting"));
        }

        private void ClientDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                CRCOptions.DisplayLocation = Location;
                CRCOptions.DisplaySize = Size;
            }
            else
            {
                CRCOptions.DisplayLocation = RestoreBounds.Location;
                CRCOptions.DisplaySize = RestoreBounds.Size;
            }

            CRCClient.Stop();
        }

        private void buttonOptions_Click(object sender, EventArgs e)
        {
            new OptionsForm().Show();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string trimmed = textBoxInput.Text.Trim();
            if (trimmed.Length > 0)
            {
                if (trimmed[0] == '/')
                {
                    CRCCommands.ProcessCommand(trimmed, this);
                }
                else if (trimmed.Length > 0)
                {
                    CRCClient.Send(trimmed);
                }
                textBoxInput.Clear();
            }
        }

        private void timerGameCheck_Tick(object sender, EventArgs e)
        {
            CRCGame.GameCheck();
        }

        private void timerGameUpdate_Tick(object sender, EventArgs e)
        {
            CRCGame.GameUpdate();
        }

        private async void timerCheckUpdate_Tick(object sender, EventArgs e)
        {
            bool result = await CRCUpdate.CheckUpdate();
        }

        private void richTextBoxMessages_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            // Apparently safer than just passing the link
            Process.Start("explorer.exe", e.LinkText);
        }


        public void Enable()
        {
            Invoke(() =>
            {
                buttonSend.Enabled = true;
                buttonOptions.Enabled = true;
            });
        }

        public void Disable()
        {
            Invoke(() =>
            {
                buttonSend.Enabled = false;
                buttonOptions.Enabled = false;
            });
        }

        private void AddLinePrefix()
        {
            if (richTextBoxMessages.Lines.Length != 0)
                richTextBoxMessages.AppendText("\r\n");
            if (CRCOptions.ShowTimestamps)
            {
                richTextBoxMessages.SelectionColor = Color.Black;
                richTextBoxMessages.SelectionFont = timeFont;
                richTextBoxMessages.AppendText(DateTime.Now.ToString("[hh:mm:ss] "));
            }
        }

        public void AddLine(string line, Color color)
        {
            Invoke(() =>
            {
                AddLinePrefix();
                richTextBoxMessages.SelectionFont = mainFont;
                richTextBoxMessages.SelectionColor = color;
                richTextBoxMessages.AppendText(line);
            });
        }

        public void AddInformation(string line)
        {
            AddLine(line, Color.DarkBlue);
        }

        public void AddError(string line)
        {
            AddLine(line, Color.Red);
        }

        public void AddMessage(string nick, string message, Color nickColor)
        {
            Invoke(() =>
            {
                AddLinePrefix();
                richTextBoxMessages.SelectionFont = boldFont;
                richTextBoxMessages.SelectionColor = nickColor;
                richTextBoxMessages.AppendText(nick + ": ");
                richTextBoxMessages.SelectionFont = mainFont;
                richTextBoxMessages.SelectionColor = Color.Black;
                richTextBoxMessages.AppendText(message);
            });
        }

        public void AddHighlightMessage(string nick, string message)
        {
            Invoke(() =>
            {
                AddMessage(nick, message, Color.Black);
                int start = richTextBoxMessages.GetFirstCharIndexOfCurrentLine();
                int length = richTextBoxMessages.TextLength - start;
                richTextBoxMessages.Select(start, length);
                richTextBoxMessages.SelectionBackColor = Color.Yellow;
                richTextBoxMessages.Select(0, 0);
                richTextBoxMessages.SelectionBackColor = Color.White;
            });
        }

        private void textBoxUsers_TextChanged(object sender, EventArgs e)
        {
            foreach (string user in CRCClient.InGameStatus.Keys)
            {
                if (textBoxUsers.Text.Contains(user))
                {
                    if (CRCClient.InGameStatus.ContainsKey(user) && CRCClient.InGameStatus[user] == "True")
                    {
                        textBoxUsers.Select(textBoxUsers.Text.IndexOf(user), user.Length);
                        textBoxUsers.SelectedText = "⦿ " + user;
                    }
                    else
                    {
                        textBoxUsers.Select(textBoxUsers.Text.IndexOf(user), user.Length);
                        textBoxUsers.SelectedText = "⦾ " + user;
                    }

                }
                else return;
            }
            MatchCollection ingame = Regex.Matches(textBoxUsers.Text, "⦿");
            MatchCollection offgame = Regex.Matches(textBoxUsers.Text, "⦾");
            foreach (Match match in ingame)
            {
                textBoxUsers.Select(match.Index, match.Length);
                textBoxUsers.SelectionColor = Color.Green;
            }
            foreach (Match match in offgame)
            {
                textBoxUsers.Select(match.Index, match.Length);
                textBoxUsers.SelectionColor = Color.Red;
            }
        }

            public void UpdateUsers(List<string> users)
        {
            Invoke(() =>
                textBoxUsers.Text = string.Join("\r\n", users
                ));
        }

        private void Invoke(Action action)
        {
            base.Invoke(action);
        }
    }
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
