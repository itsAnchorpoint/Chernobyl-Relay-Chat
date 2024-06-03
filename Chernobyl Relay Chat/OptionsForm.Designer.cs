namespace Chernobyl_Relay_Chat
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonRandom = new System.Windows.Forms.Button();
            this.comboBoxFaction = new System.Windows.Forms.ComboBox();
            this.checkBoxDeathSend = new System.Windows.Forms.CheckBox();
            this.checkBoxDeathReceive = new System.Windows.Forms.CheckBox();
            this.radioButtonFactionAuto = new System.Windows.Forms.RadioButton();
            this.radioButtonFactionManual = new System.Windows.Forms.RadioButton();
            this.numericUpDownDeath = new System.Windows.Forms.NumericUpDown();
            this.labelDeathInterval = new System.Windows.Forms.Label();
            this.labelDeathSeconds = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxTimestamps = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.linkLabelDiscord = new System.Windows.Forms.LinkLabel();
            this.labelNewsDuration = new System.Windows.Forms.Label();
            this.numericUpDownNewsDuration = new System.Windows.Forms.NumericUpDown();
            this.labelNewsSeconds = new System.Windows.Forms.Label();
            this.labelChatKey = new System.Windows.Forms.Label();
            this.textBoxChatKey = new System.Windows.Forms.TextBox();
            this.buttonChatKey = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageClient = new System.Windows.Forms.TabPage();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.checkBoxSoundToggle = new System.Windows.Forms.CheckBox();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.tabPageGame = new System.Windows.Forms.TabPage();
            this.checkBoxCloseChat = new System.Windows.Forms.CheckBox();
            this.checkBoxNewsSound = new System.Windows.Forms.CheckBox();
            this.pictureBoxDiscordLogo = new System.Windows.Forms.PictureBox();
            this.checkBoxDisableUnregisteredMessage = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDeath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNewsDuration)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageClient.SuspendLayout();
            this.tabPageGame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDiscordLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(147, 441);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(228, 441);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(6, 116);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(6, 132);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(199, 20);
            this.textBoxName.TabIndex = 3;
            // 
            // buttonRandom
            // 
            this.buttonRandom.Location = new System.Drawing.Point(211, 130);
            this.buttonRandom.Name = "buttonRandom";
            this.buttonRandom.Size = new System.Drawing.Size(75, 23);
            this.buttonRandom.TabIndex = 4;
            this.buttonRandom.Text = "Random";
            this.buttonRandom.UseVisualStyleBackColor = true;
            this.buttonRandom.Click += new System.EventHandler(this.buttonRandom_Click);
            // 
            // comboBoxFaction
            // 
            this.comboBoxFaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFaction.Enabled = false;
            this.comboBoxFaction.FormattingEnabled = true;
            this.comboBoxFaction.Items.AddRange(new object[] {
            "Bandit",
            "Clear Sky",
            "Duty",
            "Ecologist",
            "Freedom",
            "Loner",
            "Mercenary",
            "Military",
            "Monolith",
            "Renegade"});
            this.comboBoxFaction.Location = new System.Drawing.Point(6, 92);
            this.comboBoxFaction.Name = "comboBoxFaction";
            this.comboBoxFaction.Size = new System.Drawing.Size(280, 21);
            this.comboBoxFaction.TabIndex = 6;
            // 
            // checkBoxDeathSend
            // 
            this.checkBoxDeathSend.AutoSize = true;
            this.checkBoxDeathSend.Location = new System.Drawing.Point(6, 280);
            this.checkBoxDeathSend.Name = "checkBoxDeathSend";
            this.checkBoxDeathSend.Size = new System.Drawing.Size(131, 17);
            this.checkBoxDeathSend.TabIndex = 7;
            this.checkBoxDeathSend.Text = "Send death messages";
            this.checkBoxDeathSend.UseVisualStyleBackColor = true;
            // 
            // checkBoxDeathReceive
            // 
            this.checkBoxDeathReceive.AutoSize = true;
            this.checkBoxDeathReceive.Location = new System.Drawing.Point(6, 303);
            this.checkBoxDeathReceive.Name = "checkBoxDeathReceive";
            this.checkBoxDeathReceive.Size = new System.Drawing.Size(146, 17);
            this.checkBoxDeathReceive.TabIndex = 8;
            this.checkBoxDeathReceive.Text = "Receive death messages";
            this.checkBoxDeathReceive.UseVisualStyleBackColor = true;
            this.checkBoxDeathReceive.CheckedChanged += new System.EventHandler(this.checkBoxDeathReceive_CheckedChanged);
            // 
            // radioButtonFactionAuto
            // 
            this.radioButtonFactionAuto.AutoSize = true;
            this.radioButtonFactionAuto.Location = new System.Drawing.Point(6, 46);
            this.radioButtonFactionAuto.Name = "radioButtonFactionAuto";
            this.radioButtonFactionAuto.Size = new System.Drawing.Size(113, 17);
            this.radioButtonFactionAuto.TabIndex = 9;
            this.radioButtonFactionAuto.Text = "Sync game faction";
            this.toolTip1.SetToolTip(this.radioButtonFactionAuto, "Other users will see your faction as whichever one you played as last");
            this.radioButtonFactionAuto.UseVisualStyleBackColor = true;
            // 
            // radioButtonFactionManual
            // 
            this.radioButtonFactionManual.AutoSize = true;
            this.radioButtonFactionManual.Location = new System.Drawing.Point(6, 69);
            this.radioButtonFactionManual.Name = "radioButtonFactionManual";
            this.radioButtonFactionManual.Size = new System.Drawing.Size(87, 17);
            this.radioButtonFactionManual.TabIndex = 10;
            this.radioButtonFactionManual.Text = "Static faction";
            this.toolTip1.SetToolTip(this.radioButtonFactionManual, "Other users will always see you as the faction specified below");
            this.radioButtonFactionManual.UseVisualStyleBackColor = true;
            this.radioButtonFactionManual.CheckedChanged += new System.EventHandler(this.radioButtonFactionManual_CheckedChanged);
            // 
            // numericUpDownDeath
            // 
            this.numericUpDownDeath.Location = new System.Drawing.Point(6, 366);
            this.numericUpDownDeath.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numericUpDownDeath.Name = "numericUpDownDeath";
            this.numericUpDownDeath.Size = new System.Drawing.Size(46, 20);
            this.numericUpDownDeath.TabIndex = 11;
            this.numericUpDownDeath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelDeathInterval
            // 
            this.labelDeathInterval.AutoSize = true;
            this.labelDeathInterval.Location = new System.Drawing.Point(6, 350);
            this.labelDeathInterval.Name = "labelDeathInterval";
            this.labelDeathInterval.Size = new System.Drawing.Size(194, 13);
            this.labelDeathInterval.TabIndex = 12;
            this.labelDeathInterval.Text = "Minimum time between death messages";
            // 
            // labelDeathSeconds
            // 
            this.labelDeathSeconds.AutoSize = true;
            this.labelDeathSeconds.Location = new System.Drawing.Point(58, 368);
            this.labelDeathSeconds.Name = "labelDeathSeconds";
            this.labelDeathSeconds.Size = new System.Drawing.Size(47, 13);
            this.labelDeathSeconds.TabIndex = 13;
            this.labelDeathSeconds.Text = "seconds";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(6, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(280, 2);
            this.label4.TabIndex = 14;
            // 
            // checkBoxTimestamps
            // 
            this.checkBoxTimestamps.AutoSize = true;
            this.checkBoxTimestamps.Location = new System.Drawing.Point(6, 257);
            this.checkBoxTimestamps.Name = "checkBoxTimestamps";
            this.checkBoxTimestamps.Size = new System.Drawing.Size(108, 17);
            this.checkBoxTimestamps.TabIndex = 15;
            this.checkBoxTimestamps.Text = "Show timestamps";
            this.checkBoxTimestamps.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // linkLabelDiscord
            // 
            this.linkLabelDiscord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelDiscord.AutoSize = true;
            this.linkLabelDiscord.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelDiscord.Location = new System.Drawing.Point(38, 451);
            this.linkLabelDiscord.Name = "linkLabelDiscord";
            this.linkLabelDiscord.Size = new System.Drawing.Size(98, 13);
            this.linkLabelDiscord.TabIndex = 27;
            this.linkLabelDiscord.TabStop = true;
            this.linkLabelDiscord.Text = "Join CRCR Discord";
            this.toolTip1.SetToolTip(this.linkLabelDiscord, "Get the latest news and submit bugs");
            this.linkLabelDiscord.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDiscord_LinkClicked);
            // 
            // labelNewsDuration
            // 
            this.labelNewsDuration.AutoSize = true;
            this.labelNewsDuration.Location = new System.Drawing.Point(6, 88);
            this.labelNewsDuration.Name = "labelNewsDuration";
            this.labelNewsDuration.Size = new System.Drawing.Size(137, 13);
            this.labelNewsDuration.TabIndex = 17;
            this.labelNewsDuration.Text = "Duration of news messages";
            // 
            // numericUpDownNewsDuration
            // 
            this.numericUpDownNewsDuration.Location = new System.Drawing.Point(6, 104);
            this.numericUpDownNewsDuration.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownNewsDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNewsDuration.Name = "numericUpDownNewsDuration";
            this.numericUpDownNewsDuration.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numericUpDownNewsDuration.Size = new System.Drawing.Size(46, 20);
            this.numericUpDownNewsDuration.TabIndex = 18;
            this.numericUpDownNewsDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownNewsDuration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelNewsSeconds
            // 
            this.labelNewsSeconds.AutoSize = true;
            this.labelNewsSeconds.Location = new System.Drawing.Point(58, 106);
            this.labelNewsSeconds.Name = "labelNewsSeconds";
            this.labelNewsSeconds.Size = new System.Drawing.Size(47, 13);
            this.labelNewsSeconds.TabIndex = 19;
            this.labelNewsSeconds.Text = "seconds";
            // 
            // labelChatKey
            // 
            this.labelChatKey.AutoSize = true;
            this.labelChatKey.Location = new System.Drawing.Point(6, 3);
            this.labelChatKey.Name = "labelChatKey";
            this.labelChatKey.Size = new System.Drawing.Size(49, 13);
            this.labelChatKey.TabIndex = 22;
            this.labelChatKey.Text = "Chat key";
            // 
            // textBoxChatKey
            // 
            this.textBoxChatKey.Location = new System.Drawing.Point(6, 19);
            this.textBoxChatKey.Name = "textBoxChatKey";
            this.textBoxChatKey.ReadOnly = true;
            this.textBoxChatKey.Size = new System.Drawing.Size(199, 20);
            this.textBoxChatKey.TabIndex = 23;
            // 
            // buttonChatKey
            // 
            this.buttonChatKey.Location = new System.Drawing.Point(211, 17);
            this.buttonChatKey.Name = "buttonChatKey";
            this.buttonChatKey.Size = new System.Drawing.Size(75, 23);
            this.buttonChatKey.TabIndex = 24;
            this.buttonChatKey.Text = "Change";
            this.buttonChatKey.UseVisualStyleBackColor = true;
            this.buttonChatKey.Click += new System.EventHandler(this.buttonChatKey_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageClient);
            this.tabControl1.Controls.Add(this.tabPageGame);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(300, 423);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPageClient
            // 
            this.tabPageClient.Controls.Add(this.checkBoxDisableUnregisteredMessage);
            this.tabPageClient.Controls.Add(this.labelPassword);
            this.tabPageClient.Controls.Add(this.textBoxPassword);
            this.tabPageClient.Controls.Add(this.checkBoxSoundToggle);
            this.tabPageClient.Controls.Add(this.comboBoxLanguage);
            this.tabPageClient.Controls.Add(this.labelLanguage);
            this.tabPageClient.Controls.Add(this.radioButtonFactionAuto);
            this.tabPageClient.Controls.Add(this.radioButtonFactionManual);
            this.tabPageClient.Controls.Add(this.comboBoxFaction);
            this.tabPageClient.Controls.Add(this.labelName);
            this.tabPageClient.Controls.Add(this.textBoxName);
            this.tabPageClient.Controls.Add(this.buttonRandom);
            this.tabPageClient.Controls.Add(this.label4);
            this.tabPageClient.Controls.Add(this.checkBoxTimestamps);
            this.tabPageClient.Controls.Add(this.labelDeathSeconds);
            this.tabPageClient.Controls.Add(this.checkBoxDeathSend);
            this.tabPageClient.Controls.Add(this.numericUpDownDeath);
            this.tabPageClient.Controls.Add(this.labelDeathInterval);
            this.tabPageClient.Controls.Add(this.checkBoxDeathReceive);
            this.tabPageClient.Location = new System.Drawing.Point(4, 22);
            this.tabPageClient.Name = "tabPageClient";
            this.tabPageClient.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageClient.Size = new System.Drawing.Size(292, 397);
            this.tabPageClient.TabIndex = 0;
            this.tabPageClient.Text = "Client";
            this.tabPageClient.UseVisualStyleBackColor = true;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.labelPassword.Location = new System.Drawing.Point(6, 164);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(53, 13);
            this.labelPassword.TabIndex = 22;
            this.labelPassword.Text = "Password";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(7, 180);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(199, 20);
            this.textBoxPassword.TabIndex = 21;
            // 
            // checkBoxSoundToggle
            // 
            this.checkBoxSoundToggle.AutoSize = true;
            this.checkBoxSoundToggle.Location = new System.Drawing.Point(6, 326);
            this.checkBoxSoundToggle.Name = "checkBoxSoundToggle";
            this.checkBoxSoundToggle.Size = new System.Drawing.Size(116, 17);
            this.checkBoxSoundToggle.TabIndex = 20;
            this.checkBoxSoundToggle.Text = "Sound notifications";
            this.checkBoxSoundToggle.UseVisualStyleBackColor = true;
            this.checkBoxSoundToggle.CheckedChanged += new System.EventHandler(this.checkBoxSoundToggle_CheckedChanged);
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Items.AddRange(new object[] {
            "English",
            "Русский"});
            this.comboBoxLanguage.Location = new System.Drawing.Point(6, 19);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(280, 21);
            this.comboBoxLanguage.TabIndex = 17;
            // 
            // labelLanguage
            // 
            this.labelLanguage.AutoSize = true;
            this.labelLanguage.Location = new System.Drawing.Point(6, 3);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(55, 13);
            this.labelLanguage.TabIndex = 16;
            this.labelLanguage.Text = "Language";
            // 
            // tabPageGame
            // 
            this.tabPageGame.Controls.Add(this.checkBoxCloseChat);
            this.tabPageGame.Controls.Add(this.checkBoxNewsSound);
            this.tabPageGame.Controls.Add(this.labelNewsDuration);
            this.tabPageGame.Controls.Add(this.buttonChatKey);
            this.tabPageGame.Controls.Add(this.numericUpDownNewsDuration);
            this.tabPageGame.Controls.Add(this.textBoxChatKey);
            this.tabPageGame.Controls.Add(this.labelNewsSeconds);
            this.tabPageGame.Controls.Add(this.labelChatKey);
            this.tabPageGame.Location = new System.Drawing.Point(4, 22);
            this.tabPageGame.Name = "tabPageGame";
            this.tabPageGame.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGame.Size = new System.Drawing.Size(292, 374);
            this.tabPageGame.TabIndex = 1;
            this.tabPageGame.Text = "In-game";
            this.tabPageGame.UseVisualStyleBackColor = true;
            // 
            // checkBoxCloseChat
            // 
            this.checkBoxCloseChat.AutoSize = true;
            this.checkBoxCloseChat.Location = new System.Drawing.Point(6, 45);
            this.checkBoxCloseChat.Name = "checkBoxCloseChat";
            this.checkBoxCloseChat.Size = new System.Drawing.Size(140, 17);
            this.checkBoxCloseChat.TabIndex = 26;
            this.checkBoxCloseChat.Text = "Close chat after sending";
            this.checkBoxCloseChat.UseVisualStyleBackColor = true;
            // 
            // checkBoxNewsSound
            // 
            this.checkBoxNewsSound.AutoSize = true;
            this.checkBoxNewsSound.Location = new System.Drawing.Point(6, 68);
            this.checkBoxNewsSound.Name = "checkBoxNewsSound";
            this.checkBoxNewsSound.Size = new System.Drawing.Size(123, 17);
            this.checkBoxNewsSound.TabIndex = 25;
            this.checkBoxNewsSound.Text = "Play message sound";
            this.checkBoxNewsSound.UseVisualStyleBackColor = true;
            // 
            // pictureBoxDiscordLogo
            // 
            this.pictureBoxDiscordLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxDiscordLogo.Image = global::Chernobyl_Relay_Chat.Properties.Resources.discord_icon_200x200;
            this.pictureBoxDiscordLogo.Location = new System.Drawing.Point(10, 446);
            this.pictureBoxDiscordLogo.Name = "pictureBoxDiscordLogo";
            this.pictureBoxDiscordLogo.Size = new System.Drawing.Size(32, 23);
            this.pictureBoxDiscordLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxDiscordLogo.TabIndex = 26;
            this.pictureBoxDiscordLogo.TabStop = false;
            // 
            // checkBoxDisableUnregisteredMessage
            // 
            this.checkBoxDisableUnregisteredMessage.AutoSize = true;
            this.checkBoxDisableUnregisteredMessage.Location = new System.Drawing.Point(6, 234);
            this.checkBoxDisableUnregisteredMessage.Name = "checkBoxDisableUnregisteredMessage";
            this.checkBoxDisableUnregisteredMessage.Size = new System.Drawing.Size(219, 17);
            this.checkBoxDisableUnregisteredMessage.TabIndex = 23;
            this.checkBoxDisableUnregisteredMessage.Text = "Disable unregistered nickname message.";
            this.checkBoxDisableUnregisteredMessage.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(324, 481);
            this.Controls.Add(this.linkLabelDiscord);
            this.Controls.Add(this.pictureBoxDiscordLogo);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(340, 520);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(340, 520);
            this.Name = "OptionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CRCR Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDeath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNewsDuration)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageClient.ResumeLayout(false);
            this.tabPageClient.PerformLayout();
            this.tabPageGame.ResumeLayout(false);
            this.tabPageGame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDiscordLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonRandom;
        private System.Windows.Forms.ComboBox comboBoxFaction;
        private System.Windows.Forms.CheckBox checkBoxDeathSend;
        private System.Windows.Forms.CheckBox checkBoxDeathReceive;
        private System.Windows.Forms.RadioButton radioButtonFactionAuto;
        private System.Windows.Forms.RadioButton radioButtonFactionManual;
        private System.Windows.Forms.NumericUpDown numericUpDownDeath;
        private System.Windows.Forms.Label labelDeathInterval;
        private System.Windows.Forms.Label labelDeathSeconds;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxTimestamps;
        private System.Windows.Forms.Label labelNewsDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownNewsDuration;
        private System.Windows.Forms.Label labelNewsSeconds;
        private System.Windows.Forms.Label labelChatKey;
        private System.Windows.Forms.TextBox textBoxChatKey;
        private System.Windows.Forms.Button buttonChatKey;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageClient;
        private System.Windows.Forms.TabPage tabPageGame;
        private System.Windows.Forms.CheckBox checkBoxCloseChat;
        private System.Windows.Forms.CheckBox checkBoxNewsSound;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.CheckBox checkBoxSoundToggle;
        private System.Windows.Forms.PictureBox pictureBoxDiscordLogo;
        private System.Windows.Forms.LinkLabel linkLabelDiscord;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.CheckBox checkBoxDisableUnregisteredMessage;
    }
}