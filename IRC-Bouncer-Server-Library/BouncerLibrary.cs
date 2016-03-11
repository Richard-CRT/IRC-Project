using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncerServerLibraryNS
{
    public delegate void BouncerLibraryMessageCallBack(string result, bool receive);

    public class BouncerServerLibrary
    {
        public AsynchronousSocketListener SocketConnection { get; set; }
        public int Port { get; set; }
        
        public BouncerServerLibrary(int port)
        {
            Port = port;
        }

        public void CommandHandler(string message, bool receive)
        {
            if (receive)
            {
                Console.WriteLine("BOUNCER ← " + message);
            }
            else
            {
                Console.WriteLine("BOUNCER → " + message);
            }
        }

        public void Connect()
        {
            BouncerLibraryMessageCallBack callback = CommandHandler;
            SocketConnection = new AsynchronousSocketListener(callback, Port);
            SocketConnection.StartListening();
        }
    }
}
