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
    public class ChannelUserClass
    {
        private IRCUser Client;
        public int ID { get; set; }
        public string Name
        {
            get
            {
                return Client.Users[this.UserIndex].Name;
            }
            set
            {
                Client.Users[this.UserIndex].Name = value;
            }
        }
        public string NameLower
        {
            get
            {
                return Client.Users[this.UserIndex].NameLower;
            }
            set
            {
                Client.Users[this.UserIndex].NameLower = value;
            }
        }
        public char IRCType
        {
            get
            {
                return Client.Users[this.UserIndex].IRCType;
            }
            set
            {
                Client.Users[this.UserIndex].IRCType = value;
            }
        }
        public int Power
        {
            get
            {
                return Client.Users[this.UserIndex].Power;
            }
            set
            {
                Client.Users[this.UserIndex].Power = value;
            }
        }
        public int UserIndex
        {
            get
            {
                return Client.findUserIndex(this.ID);
            }
        }

        public ChannelUserClass(IRCUser client, string name, string ident, string server)
        {
            Client = client;
            int index = client.findUserIndex(name);
            if (index == -1)
            {
                int userIndex = client.Users.Count();
                client.Users.Add(new UserClass(client, name, ident, server));
                ID = client.Users[userIndex].ID;
            }
            else
            {
                ID = client.Users[index].ID;
            }
        }
    }
}
