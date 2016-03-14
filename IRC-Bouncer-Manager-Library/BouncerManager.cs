using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BouncerServerLibraryNS;
using IRCLibrary;

namespace BouncerServerManagerNS
{
    public class BouncerManager
    {
        private string Nick;
        private string Server;
        private string Ident;
        private string Hostname;
        public List<string> DefaultChannels;

        public IRCUser Client { get; set; }
        public BouncerServerLibrary Bouncer { get; set; }

        private void inputCMD(IRCUser client)
        {
            string command;
            while (true)
            {
                command = Console.ReadLine();
                if (command.ToUpper() == "EXIT")
                {
                    if (client.SocketConnection.connected)
                    {
                        client.SendRaw("QUIT");
                    }
                    Console.Write("Press enter to exit... ");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
                else
                {
                    client.SendRaw(command);
                }
            }
        }

        private void ClientThread(IRCUser Client)
        {
            Client.Connect();
        }

        private void BouncerThread(BouncerServerLibrary Bouncer)
        {
            Bouncer.Connect();
        }

        public BouncerManager()
        {
            Nick = "DefaultNick";
            Server = "irc.quakenet.org";
            Ident = "IRCBot";
            Hostname = "localhost";
            DefaultChannels = new List<string>();
            if (File.Exists("config.xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("config.xml");
                Nick = doc["config"]["user"]["nick"].InnerText;
                Ident = doc["config"]["user"]["ident"].InnerText;
                Server = doc["config"]["connection"]["server"].InnerText;
                Hostname = doc["config"]["connection"]["hostname"].InnerText;
                for (int i = 0; i < doc["config"]["user"]["channels"].ChildNodes.Count; i++)
                {
                    DefaultChannels.Add(doc["config"]["user"]["channels"].ChildNodes[i].InnerText);
                }
            }

            Client = new IRCUser(Nick, Server, 6667, DefaultChannels, Ident, Hostname, "", "", false);
            Bouncer = new BouncerServerLibrary(Client, 8889);

            Thread t = new Thread(() => { inputCMD(Client); });
            t.IsBackground = true;

            Thread clientThread = new Thread(() => { ClientThread(Client); });
            clientThread.IsBackground = true;

            Thread bouncerThread = new Thread(() => { BouncerThread(Bouncer); });
            bouncerThread.IsBackground = true;

            t.Start();
            bouncerThread.Start();
            clientThread.Start();
        }
    }
}
