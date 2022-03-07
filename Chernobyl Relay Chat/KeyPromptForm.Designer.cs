namespace Chernobyl_Relay_Chat
{
    partial class KeyPromptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyPromptForm));
            this.labelError = new System.Windows.Forms.Label();
            this.labelHelp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelError.Location = new System.Drawing.Point(51, 85);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(266, 13);
            this.labelError.TabIndex = 1;
            this.labelError.Text = "Sorry, that key is not supported by STALKER.";
            // 
            // labelHelp
            // 
            this.labelHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHelp.Location = new System.Drawing.Point(0, 0);
            this.labelHelp.Name = "labelHelp";
            this.labelHelp.Size = new System.Drawing.Size(369, 127);
            this.labelHelp.TabIndex = 0;
            this.labelHelp.Text = "Press the desired key now to change your in-game chat button.\r\n\r\nModifiers such a" +
    "s Alt, Ctrl, or Shift are not supported.\r\n\r\n";
            this.labelHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KeyPromptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 127);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.labelHelp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "KeyPromptForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chernobyl Relay Chat Rebirth";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyPromptForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Label labelHelp;
    }
}