using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BouncerClientLibraryNS
{
    public delegate void BouncerLibraryMessageCallBack(string result, bool receive);

    public class BouncerClientLibrary
    {
        public AsynchronousClient SocketConnection { get; set; }
        public int Port { get; set; }
        public string IP { get; set; }
        public Form MainForm { get; set; }
        public Control RunButton { get; set; }
        public Control TextboxEdit { get; set; }
        public Control TextboxInput { get; set; }

        public BouncerClientLibrary(Form mainForm, string ip, int port)
        {
            IP = ip;
            Port = port;
            MainForm = mainForm;
            foreach (Control control in mainForm.Controls)
            {
                switch (control.Name)
                {
                    case "ButtonRun":
                        RunButton = control;
                        break;
                    case "TextboxEdit":
                        TextboxEdit = control;
                        break;
                    case "TextboxInput":
                        TextboxInput = control;
                        break;
                }
            }
            BouncerLibraryMessageCallBack callback = CommandHandler;
        }

        public void CommandHandler(string message, bool receive)
        {
            if (receive)
            {
                Log("BOUNCER ← " + message);
            }
            else
            {
                Log("BOUNCER → " + message);
            }
        }

        public void Connect()
        {
            TextboxEdit.Invoke(new Action(() => TextboxEdit.Text = "Connecting..."));
            BouncerLibraryMessageCallBack callback = CommandHandler;
            SocketConnection = new AsynchronousClient(callback, IP, Port);
            SocketConnection.Start();
        }

        private void Log(string message)
        {
            TextboxEdit.Invoke(new Action(() => TextboxEdit.Text = TextboxEdit.Text + Environment.NewLine + message));
        }
    }
}
