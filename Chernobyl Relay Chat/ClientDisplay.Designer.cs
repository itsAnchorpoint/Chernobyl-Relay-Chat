namespace Chernobyl_Relay_Chat
{
    partial class ClientDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientDisplay));
            this.richTextBoxMessages = new System.Windows.Forms.RichTextBox();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.timerGameCheck = new System.Windows.Forms.Timer(this.components);
            this.timerGameUpdate = new System.Windows.Forms.Timer(this.components);
            this.buttonOptions = new System.Windows.Forms.Button();
            this.textBoxUsers = new System.Windows.Forms.RichTextBox();
            this.timerCheckUpdate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // richTextBoxMessages
            // 
            resources.ApplyResources(this.richTextBoxMessages, "richTextBoxMessages");
            this.richTextBoxMessages.HideSelection = false;
            this.richTextBoxMessages.Name = "richTextBoxMessages";
            this.richTextBoxMessages.ReadOnly = true;
            this.richTextBoxMessages.TabStop = false;
            this.richTextBoxMessages.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxMessages_LinkClicked);
            // 
            // textBoxInput
            // 
            resources.ApplyResources(this.textBoxInput, "textBoxInput");
            this.textBoxInput.Name = "textBoxInput";
            // 
            // buttonSend
            // 
            resources.ApplyResources(this.buttonSend, "buttonSend");
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // timerGameCheck
            // 
            this.timerGameCheck.Enabled = true;
            this.timerGameCheck.Interval = 10000;
            this.timerGameCheck.Tick += new System.EventHandler(this.timerGameCheck_Tick);
            // 
            // timerGameUpdate
            // 
            this.timerGameUpdate.Enabled = true;
            this.timerGameUpdate.Interval = 500;
            this.timerGameUpdate.Tick += new System.EventHandler(this.timerGameUpdate_Tick);
            // 
            // buttonOptions
            // 
            resources.ApplyResources(this.buttonOptions, "buttonOptions");
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.buttonOptions_Click);
            // 
            // textBoxUsers
            // 
            resources.ApplyResources(this.textBoxUsers, "textBoxUsers");
            this.textBoxUsers.Name = "textBoxUsers";
            this.textBoxUsers.ReadOnly = true;
            this.textBoxUsers.TextChanged += new System.EventHandler(this.textBoxUsers_TextChanged);
            // 
            // timerCheckUpdate
            // 
            this.timerCheckUpdate.Enabled = true;
            this.timerCheckUpdate.Interval = 600000;
            this.timerCheckUpdate.Tick += new System.EventHandler(this.timerCheckUpdate_Tick);
            // 
            // ClientDisplay
            // 
            this.AcceptButton = this.buttonSend;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxUsers);
            this.Controls.Add(this.buttonOptions);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.richTextBoxMessages);
            this.Name = "ClientDisplay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientDisplay_FormClosing);
            this.Load += new System.EventHandler(this.ClientDisplay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMessages;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Timer timerGameCheck;
        private System.Windows.Forms.Timer timerGameUpdate;
        private System.Windows.Forms.Button buttonOptions;
        private System.Windows.Forms.RichTextBox textBoxUsers;
        private System.Windows.Forms.Timer timerCheckUpdate;
    }
}

