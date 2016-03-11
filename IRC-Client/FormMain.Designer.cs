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
            this.ButtonSend = new System.Windows.Forms.Button();
            this.TextboxEdit = new System.Windows.Forms.TextBox();
            this.TextboxInput = new System.Windows.Forms.TextBox();
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonSend
            // 
            this.ButtonSend.Location = new System.Drawing.Point(131, 411);
            this.ButtonSend.Name = "ButtonSend";
            this.ButtonSend.Size = new System.Drawing.Size(92, 23);
            this.ButtonSend.TabIndex = 0;
            this.ButtonSend.Text = "Send";
            this.ButtonSend.UseVisualStyleBackColor = true;
            this.ButtonSend.Click += new System.EventHandler(this.ButtonSend_Click);
            // 
            // TextboxEdit
            // 
            this.TextboxEdit.Location = new System.Drawing.Point(17, 19);
            this.TextboxEdit.Multiline = true;
            this.TextboxEdit.Name = "TextboxEdit";
            this.TextboxEdit.Size = new System.Drawing.Size(510, 357);
            this.TextboxEdit.TabIndex = 1;
            // 
            // TextboxInput
            // 
            this.TextboxInput.Location = new System.Drawing.Point(17, 383);
            this.TextboxInput.Name = "TextboxInput";
            this.TextboxInput.Size = new System.Drawing.Size(510, 22);
            this.TextboxInput.TabIndex = 2;
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.Location = new System.Drawing.Point(17, 411);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(108, 23);
            this.ButtonConnect.TabIndex = 3;
            this.ButtonConnect.Text = "Connect";
            this.ButtonConnect.UseVisualStyleBackColor = true;
            this.ButtonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 447);
            this.Controls.Add(this.ButtonConnect);
            this.Controls.Add(this.TextboxInput);
            this.Controls.Add(this.TextboxEdit);
            this.Controls.Add(this.ButtonSend);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonSend;
        private System.Windows.Forms.TextBox TextboxEdit;
        private System.Windows.Forms.TextBox TextboxInput;
        private System.Windows.Forms.Button ButtonConnect;
    }
}