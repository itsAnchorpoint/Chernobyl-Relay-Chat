namespace Chernobyl_Relay_Chat
{
    partial class UpdateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelGamedata = new System.Windows.Forms.Label();
            this.labelReleaseNotes = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // buttonDownload
            // 
            this.buttonDownload.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDownload.Location = new System.Drawing.Point(348, 601);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(75, 23);
            this.buttonDownload.TabIndex = 0;
            this.buttonDownload.Text = "Download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(429, 601);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(12, 9);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(447, 39);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = resources.GetString("labelDescription.Text");
            // 
            // labelGamedata
            // 
            this.labelGamedata.AutoSize = true;
            this.labelGamedata.Location = new System.Drawing.Point(12, 57);
            this.labelGamedata.Name = "labelGamedata";
            this.labelGamedata.Size = new System.Drawing.Size(78, 13);
            this.labelGamedata.TabIndex = 3;
            this.labelGamedata.Text = "labelGamedata";
            // 
            // labelReleaseNotes
            // 
            this.labelReleaseNotes.AutoSize = true;
            this.labelReleaseNotes.Location = new System.Drawing.Point(12, 79);
            this.labelReleaseNotes.Name = "labelReleaseNotes";
            this.labelReleaseNotes.Size = new System.Drawing.Size(75, 13);
            this.labelReleaseNotes.TabIndex = 4;
            this.labelReleaseNotes.Text = "Release notes";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 95);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(492, 500);
            this.webBrowser1.TabIndex = 5;
            // 
            // UpdateForm
            // 
            this.AcceptButton = this.buttonDownload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(516, 637);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.labelReleaseNotes);
            this.Controls.Add(this.labelGamedata);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonDownload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdateForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Chernobyl Relay Chat Rebirth";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelGamedata;
        private System.Windows.Forms.Label labelReleaseNotes;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}