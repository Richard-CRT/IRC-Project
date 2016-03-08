using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRCLibrary;

namespace IRCLibrary
{
    public class ChannelClass
    {
        public List<ChannelUserClass> Users { get; set; } = new List<ChannelUserClass>();
        public string Name { get; set; }
        public string NameLower { get; set; }
        private IRCUser Client { get; set; }

        public ChannelClass(IRCUser client, string channelName, List<string> users)
        {
            Name = channelName;
            NameLower = channelName.ToLower();
            Client = client;
            foreach (string userName in users)
            {
                Users.Add(new ChannelUserClass(client, userName, null, null));
            }
            Client.SendRaw("WHO " + string.Join(",", users.ConvertAll(s => s.TrimStart(client.userPrefixes))));
        }

        public void SendMessage(string message)
        {
            Client.SendRaw("PRIVMSG " + Name + " :" + message);
        }
    }
}
