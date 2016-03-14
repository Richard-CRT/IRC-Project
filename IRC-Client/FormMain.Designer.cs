using System;

namespace IRC_Client
{
    partial class FormMain
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
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeViewServers = new System.Windows.Forms.TreeView();
            this.RTextBoxChat = new System.Windows.Forms.RichTextBox();
            this.TextBoxInput = new System.Windows.Forms.TextBox();
            this.ButtonSend = new System.Windows.Forms.Button();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.Location = new System.Drawing.Point(13, 33);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(108, 32);
            this.ButtonConnect.TabIndex = 3;
            this.ButtonConnect.Text = "Connect";
            this.ButtonConnect.UseVisualStyleBackColor = true;
            this.ButtonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // MenuStrip
            // 
            this.MenuStrip.BackColor = System.Drawing.Color.Transparent;
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1277, 28);
            this.MenuStrip.TabIndex = 4;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(44, 24);
            this.MenuFile.Text = "File";
            // 
            // TreeViewServers
            // 
            this.TreeViewServers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TreeViewServers.Location = new System.Drawing.Point(14, 77);
            this.TreeViewServers.Name = "TreeViewServers";
            this.TreeViewServers.Size = new System.Drawing.Size(104, 476);
            this.TreeViewServers.TabIndex = 5;
            // 
            // RTextBoxChat
            // 
            this.RTextBoxChat.BackColor = System.Drawing.SystemColors.Window;
            this.RTextBoxChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RTextBoxChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RTextBoxChat.Location = new System.Drawing.Point(133, 34);
            this.RTextBoxChat.Name = "RTextBoxChat";
            this.RTextBoxChat.ReadOnly = true;
            this.RTextBoxChat.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.RTextBoxChat.Size = new System.Drawing.Size(1128, 472);
            this.RTextBoxChat.TabIndex = 6;
            this.RTextBoxChat.Text = "";
            // 
            // TextBoxInput
            // 
            this.TextBoxInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxInput.Location = new System.Drawing.Point(133, 523);
            this.TextBoxInput.Name = "TextBoxInput";
            this.TextBoxInput.Size = new System.Drawing.Size(1001, 30);
            this.TextBoxInput.TabIndex = 7;
            // 
            // ButtonSend
            // 
            this.ButtonSend.Location = new System.Drawing.Point(1148, 521);
            this.ButtonSend.Name = "ButtonSend";
            this.ButtonSend.Size = new System.Drawing.Size(113, 31);
            this.ButtonSend.TabIndex = 8;
            this.ButtonSend.Text = "Send";
            this.ButtonSend.UseVisualStyleBackColor = true;
            this.ButtonSend.Click += new System.EventHandler(this.ButtonSend_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 565);
            this.Controls.Add(this.ButtonSend);
            this.Controls.Add(this.TextBoxInput);
            this.Controls.Add(this.RTextBoxChat);
            this.Controls.Add(this.TreeViewServers);
            this.Controls.Add(this.ButtonConnect);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ButtonConnect;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.TreeView TreeViewServers;
        private System.Windows.Forms.RichTextBox RTextBoxChat;
        private System.Windows.Forms.TextBox TextBoxInput;
        private System.Windows.Forms.Button ButtonSend;
    }
}