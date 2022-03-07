namespace Chernobyl_Relay_Chat
{
    partial class LanguagePrompt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguagePrompt));
            this.label2 = new System.Windows.Forms.Label();
            this.buttonEnglish = new System.Windows.Forms.Button();
            this.buttonRussian = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(375, 202);
            this.label2.TabIndex = 1;
            this.label2.Text = resources.GetString("label2.Text");
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonEnglish
            // 
            this.buttonEnglish.Location = new System.Drawing.Point(69, 141);
            this.buttonEnglish.Name = "buttonEnglish";
            this.buttonEnglish.Size = new System.Drawing.Size(75, 23);
            this.buttonEnglish.TabIndex = 2;
            this.buttonEnglish.Text = "English";
            this.buttonEnglish.UseVisualStyleBackColor = true;
            this.buttonEnglish.Click += new System.EventHandler(this.buttonEnglish_Click);
            // 
            // buttonRussian
            // 
            this.buttonRussian.Location = new System.Drawing.Point(231, 141);
            this.buttonRussian.Name = "buttonRussian";
            this.buttonRussian.Size = new System.Drawing.Size(75, 23);
            this.buttonRussian.TabIndex = 3;
            this.buttonRussian.Text = "Русский";
            this.buttonRussian.UseVisualStyleBackColor = true;
            this.buttonRussian.Click += new System.EventHandler(this.buttonRussian_Click);
            // 
            // LanguagePrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 202);
            this.ControlBox = false;
            this.Controls.Add(this.buttonRussian);
            this.Controls.Add(this.buttonEnglish);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LanguagePrompt";
            this.Text = "Chernobyl Relay Chat Rebirth";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonEnglish;
        private System.Windows.Forms.Button buttonRussian;
    }
}