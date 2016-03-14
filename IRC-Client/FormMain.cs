using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BouncerClientLibraryNS;

namespace IRC_Client
{
    public partial class FormMain : Form
    {
        BouncerClientLibrary Bouncer;
        public Dictionary<string,string> Logs;

        public FormMain()
        {
            InitializeComponent();
            Bouncer = new BouncerClientLibrary(this);
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            if (Bouncer.SocketConnection != null)
            {
                Bouncer.SocketConnection.client.Close();
            }
            Bouncer.Connect("192.168.0.111", 8889);
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            Bouncer.SocketConnection.Send(TextBoxInput.Text);
        }

        public void Log(string message)
        {
            if (RTextBoxChat.Text == "")
            {
                RTextBoxChat.AppendText(message);
            }
            else
            {
                RTextBoxChat.AppendText(Environment.NewLine + message);
            }
        }
    }
}
