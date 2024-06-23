using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace Chernobyl_Relay_Chat
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            Text = CRCStrings.Localize("options_title");
            buttonOK.Text = CRCStrings.Localize("options_ok");
            buttonCancel.Text = CRCStrings.Localize("options_cancel");
            tabControl1.TabPages[0].Text = CRCStrings.Localize("options_tab_client");
            tabControl1.TabPages[1].Text = CRCStrings.Localize("options_tab_game");

            labelLanguage.Text = CRCStrings.Localize("options_language");
            radioButtonFactionAuto.Text = CRCStrings.Localize("options_auto_faction");
            radioButtonFactionManual.Text = CRCStrings.Localize("options_manual_faction");
            labelName.Text = CRCStrings.Localize("options_name");
            labelPassword.Text = CRCStrings.Localize("options_password");
            buttonRandom.Text = CRCStrings.Localize("options_name_random");
            checkBoxBlockPayment.Text = CRCStrings.Localize("options_disable_payment");
            checkBoxDisableUnregisteredMessage.Text = CRCStrings.Localize("options_disable_unregistered");
            checkBoxTimestamps.Text = CRCStrings.Localize("options_timestamps");
            checkBoxDeathSend.Text = CRCStrings.Localize("options_send_deaths");
            checkBoxDeathReceive.Text = CRCStrings.Localize("options_receive_deaths");
            labelDeathInterval.Text = CRCStrings.Localize("options_death_interval");
            labelDeathSeconds.Text = CRCStrings.Localize("crc_seconds");

            labelNewsDuration.Text = CRCStrings.Localize("options_news_duration");
            labelNewsSeconds.Text = CRCStrings.Localize("crc_seconds");
            labelChatKey.Text = CRCStrings.Localize("options_chat_key");
            buttonChatKey.Text = CRCStrings.Localize("options_chat_key_change");
            checkBoxNewsSound.Text = CRCStrings.Localize("options_news_sound");
            checkBoxCloseChat.Text = CRCStrings.Localize("options_close_chat");
            checkBoxSoundToggle.Text = CRCStrings.Localize("options_sound_notif");

            linkLabelDiscord.Text = CRCStrings.Localize("options_discord_link");
            toolTip1.SetToolTip(linkLabelDiscord, CRCStrings.Localize("options_discord_tooltip"));
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            comboBoxLanguage.SelectedIndex = languageToIndex[CRCOptions.Language];           
            radioButtonFactionAuto.Checked = CRCOptions.AutoFaction;
            radioButtonFactionManual.Checked = !CRCOptions.AutoFaction;
            textBoxName.Text = CRCOptions.Name;
            textBoxPassword.Text = CRCOptions.Password;
            comboBoxFaction.SelectedIndex = factionToIndex[CRCOptions.ManualFaction];
            checkBoxBlockPayment.Checked = CRCOptions.BlockPayments;
            checkBoxDisableUnregisteredMessage.Checked = CRCOptions.DisableUnregisteredMessage;
            checkBoxTimestamps.Checked = CRCOptions.ShowTimestamps;
            checkBoxDeathSend.Checked = CRCOptions.SendDeath;
            checkBoxDeathReceive.Checked = CRCOptions.ReceiveDeath;
            numericUpDownDeath.Value = CRCOptions.DeathInterval;
            checkBoxSoundToggle.Checked = CRCOptions.SoundNotifications;

            numericUpDownNewsDuration.Value = CRCOptions.NewsDuration;
            textBoxChatKey.Text = CRCOptions.ChatKey;
            checkBoxCloseChat.Checked = CRCOptions.CloseChat;
            checkBoxNewsSound.Checked = CRCOptions.NewsSound;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text.Replace(' ', '_');
            string result = CRCStrings.ValidateNick(name);
            string assword = textBoxPassword.Text;
            if (assword.IndexOf(' ') > 0) {
                MessageBox.Show(assword, CRCStrings.Localize("crc_error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (result != null)
            {
                MessageBox.Show(result, CRCStrings.Localize("crc_error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string lang = indexToLanguage[comboBoxLanguage.SelectedIndex];
            if(lang != CRCOptions.Language)
            {
                CRCOptions.Language = lang;
                MessageBox.Show(CRCStrings.Localize("options_language_restart"), CRCStrings.Localize("crc_name"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
            CRCOptions.AutoFaction = radioButtonFactionAuto.Checked;
            CRCOptions.ManualFaction = indexToFaction[comboBoxFaction.SelectedIndex];
            CRCOptions.Name = name;
            CRCOptions.Password = assword;
            CRCOptions.ShowTimestamps = checkBoxTimestamps.Checked;
            CRCOptions.SendDeath = checkBoxDeathSend.Checked;
            CRCOptions.ReceiveDeath = checkBoxDeathReceive.Checked;
            CRCOptions.DeathInterval = (int)numericUpDownDeath.Value;
            CRCOptions.SoundNotifications = checkBoxSoundToggle.Checked;

            CRCOptions.NewsDuration = (int)numericUpDownNewsDuration.Value;
            CRCOptions.ChatKey = textBoxChatKey.Text;
            CRCOptions.NewsSound = checkBoxNewsSound.Checked;
            CRCOptions.CloseChat = checkBoxCloseChat.Checked;
            CRCOptions.DisableUnregisteredMessage = checkBoxDisableUnregisteredMessage.Checked;
            CRCOptions.BlockPayments = checkBoxBlockPayment.Checked;

            CRCOptions.Save();
            CRCClient.UpdateSettings();
            CRCGame.UpdateSettings();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            string faction = radioButtonFactionAuto.Checked ? CRCOptions.GameFaction : indexToFaction[comboBoxFaction.SelectedIndex];
            textBoxName.Text = CRCStrings.RandomIrcName(faction);
        }

        private void radioButtonFactionManual_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxFaction.Enabled = radioButtonFactionManual.Checked;
        }

        private void checkBoxDeathReceive_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownDeath.Enabled = checkBoxDeathReceive.Checked;
        }

        private void checkBoxSoundToggle_CheckedChanged(object sender, EventArgs e)
        {
            CRCOptions.SoundNotifications = checkBoxSoundToggle.Checked;
        }

        private void buttonChatKey_Click(object sender, EventArgs e)
        {
            using (KeyPromptForm keyPromptForm = new KeyPromptForm())
            {
                keyPromptForm.ShowDialog();
                if (keyPromptForm.Result != null)
                    textBoxChatKey.Text = keyPromptForm.Result;
            }
        }

        private readonly Dictionary<string, int> languageToIndex = new Dictionary<string, int>()
        {
            ["eng"] = 0,
            ["rus"] = 1,
        };

        private readonly Dictionary<int, string> indexToLanguage = new Dictionary<int, string>()
        {
            [0] = "eng",
            [1] = "rus",
        };

        private readonly Dictionary<string, int> factionToIndex = new Dictionary<string, int>()
        {
            ["actor_bandit"] = 0,
            ["actor_csky"] = 1,
            ["actor_dolg"] = 2,
            ["actor_ecolog"] = 3,
            ["actor_freedom"] = 4,
            ["actor_stalker"] = 5,
            ["actor_killer"] = 6,
            ["actor_army"] = 7,
            ["actor_monolith"] = 8,
            ["actor_renegade"] = 9,
        };

        private readonly Dictionary<int, string> indexToFaction = new Dictionary<int, string>()
        {
            [0] = "actor_bandit",
            [1] = "actor_csky",
            [2] = "actor_dolg",
            [3] = "actor_ecolog",
            [4] = "actor_freedom",
            [5] = "actor_stalker",
            [6] = "actor_killer",
            [7] = "actor_army",
            [8] = "actor_monolith",
            [9] = "actor_renegade",
        };

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
        }

        private void linkLabelDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", "https://discord.gg/KjNHXCkHr9");
        }
    }
}
