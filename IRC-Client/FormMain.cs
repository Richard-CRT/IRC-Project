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
        public Thread BouncerClientThread;
        BouncerClientLibrary Bouncer;

        public FormMain()
        {
            InitializeComponent();

            BouncerClientThread = new Thread(() => { bouncerClientThread(); });
            BouncerClientThread.IsBackground = true;
        }

        private void bouncerClientThread()
        {
            Bouncer = new BouncerClientLibrary(this,"192.168.0.111", 8889);
            Bouncer.Connect();
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            BouncerClientThread.Start();
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            Bouncer.SocketConnection.Send("Test");
        }
    }
}
