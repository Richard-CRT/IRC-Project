using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using IRCLibrary;

namespace IRCLibrary
{
    public class UserClass
    {
        private char[] firstChars = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private IRCUser Client { get; set; }
        public string Name { get; set; }
        public string NameLower { get; set; }
        public string Ident { get; set; }
        public string Server { get; set; }
        public char IRCType { get; set; }
        public int Power { get; set; }
        public int ID { get; set; }

        public UserClass(IRCUser client, string name, string ident, string server)
        {
            Client = client;
            string temp = name.TrimStart(client.userPrefixes);
            if (temp != name)
            {
                IRCType = name[0];
            }
            else
            {
                IRCType = ' ';
            }
            Name = temp;
            NameLower = temp.ToLower();
            Ident = ident;
            Server = server;
            if (ident == null | server == null)
            {
                if (name.ToLower() == client.NickLower)
                {
                    Ident = client.Ident;
                    Server = client.IP;
                }
            }
            Power = 1;
            int highestID = 0;
            foreach (UserClass user in Client.Users)
            {
                if (user.ID > highestID)
                {
                    highestID = user.ID;
                }
            }
            ID = highestID + 1;
        }

        public void SendMessage(string message)
        {
            Client.SendRaw("PRIVMSG " + Name + " :" + message);
        }
    }
}
