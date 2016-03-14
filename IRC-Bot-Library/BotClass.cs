using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.IO;
using IRCLibrary;
using IRCLibrary.handlers;

namespace BotLibrary
{

    public class Bot
    {
        public List<CommandObserverClass> CommandObservers { get; set; } = new List<CommandObserverClass>();
        private string Nick;
        private string Server;
        private string Ident;
        private string Hostname;
        private string AuthUsername;
        private string AuthPassword;
        public List<string> DefaultChannels;
        public List<Tuple<string,string,int>> Passwords;
        
        public BotUserCommandHandler botUserCommandHandler;

        static void inputCMD(IRCUser client)
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

        public IRCUser Client { get; set; }

        public Bot()
        {
            Nick = "DefaultNick";
            Server = "irc.quakenet.org";
            Ident = "IRCBot";
            Hostname = "localhost";
            DefaultChannels = new List<string>();
            Passwords = new List<Tuple<string, string, int>>();
            if (File.Exists("config.xml"))
            {
				XmlDocument doc = new XmlDocument();
                doc.Load("config.xml");
                Nick = doc["config"]["user"]["nick"].InnerText;
                Ident = doc["config"]["user"]["ident"].InnerText;
                Server = doc["config"]["connection"]["server"].InnerText;
                Hostname = doc["config"]["connection"]["hostname"].InnerText;
                for (int i = 0; i < doc["config"]["powers"].ChildNodes.Count; i++)
                {
                    Passwords.Add(Tuple.Create(doc["config"]["powers"].ChildNodes[i]["name"].InnerText, doc["config"]["powers"].ChildNodes[i]["password"].InnerText, i + 1));
                }
                for (int i = 0; i < doc["config"]["user"]["channels"].ChildNodes.Count; i++)
                {
                    DefaultChannels.Add(doc["config"]["user"]["channels"].ChildNodes[i].InnerText);
                }
                AuthUsername = doc["config"]["user"]["auth"]["username"].InnerText;
                AuthPassword = doc["config"]["user"]["auth"]["password"].InnerText;
                if (AuthUsername == "" | AuthPassword == "")
                {
                    AuthUsername = null;
                    AuthPassword = null;
                }
            }

            Thread t = new Thread(() => { inputCMD(Client); });
            t.IsBackground = true;

            Client = new IRCUser(Nick, Server, 6667, DefaultChannels, Ident, Hostname, AuthUsername, AuthPassword, false);

            botUserCommandHandler = new BotUserCommandHandler(this);
            t.Start();

            Client.Connect();
        }

        public void AddCommand(CommandHandlerCallBack callBack, bool matchAll, string command, string usage, string description, int minimumParams, int requiredPower, int mediumType)
        {
            CommandObservers.Add(new CommandObserverClass(callBack, matchAll, command, usage, description, minimumParams, requiredPower, mediumType));
        }
    }
}
