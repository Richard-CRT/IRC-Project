using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BouncerClientLibraryNS
{
    public delegate void BouncerLibraryMessageCallBack(string result, bool receive);

    public class BouncerClientLibrary
    {
        public AsynchronousClient SocketConnection = null;
        public dynamic MainForm { get; set; }
        public Button ButtonConnect { get; set; }
        public RichTextBox RTextBoxChat { get; set; }
        public TextBox TextBoxInput { get; set; }

        public BouncerClientLibrary(Form mainForm)
        {
            MainForm = mainForm;
            foreach (Control control in mainForm.Controls)
            {
                switch (control.Name)
                {
                    case "ButtonConnect":
                        ButtonConnect = (Button)control;
                        break;
                    case "RTextBoxChat":
                        RTextBoxChat = (RichTextBox)control;
                        break;
                    case "TextBoxInput":
                        TextBoxInput = (TextBox)control;
                        break;
                }
            }
            BouncerLibraryMessageCallBack callback = CommandHandler;
        }

        public void CommandHandler(string message, bool receive)
        {
            if (receive)
            {
                MainForm.Invoke(new Action(() => MainForm.Log("BOUNCER ← " + message)));
            }
            else
            {
                MainForm.Invoke(new Action(() => MainForm.Log("BOUNCER → " + message)));
            }
        }

        public void Connect(string IP, int Port)
        {
            MainForm.Log("Connecting...");
            BouncerLibraryMessageCallBack callback = CommandHandler;
            SocketConnection = new AsynchronousClient(callback, IP, Port);
            SocketConnection.Start();
        }
    }
}
