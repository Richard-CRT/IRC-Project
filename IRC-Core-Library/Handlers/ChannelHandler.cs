using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IRCLibrary;

namespace IRCLibrary
{

    public class ChannelHandler
    {
        private IRCUser Client;
        public ChannelHandler(IRCUser lClient)
        {
            Client = lClient;
            List<Tuple<int, string>> trigger;

            // Join Channel on start
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0,":.*? 376 "+Regex.Escape(lClient.Nick)+".*"),
            };
            lClient.AddHandler(trigger, joinChannelDefault, true);

            // Setup names list on join
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0,".*?353.*?"+Regex.Escape(lClient.Nick)+".*?(#[^ ]*).*?(:.*)"),
            };
            lClient.AddHandler(trigger, setupNamesOnJoin, true);

            // Update names list on user join
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0,":([^!]*)!([^@]*)@([^ ]*) JOIN (#[^ ]*).*"),
            };
            lClient.AddHandler(trigger, updateNamesOnJoin, true);

            // Update names list on user part
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0,":(.*?)![^ ]* PART (#[^ ]*).*"),
            };
            lClient.AddHandler(trigger, updateNamesOnPart, true);

            // Update names list on user quit
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0,":(.*?)![^ ]* QUIT.*"),
            };
            lClient.AddHandler(trigger, updateNamesOnQuit, true);

            // Update names list on user nick change
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0,":([^!]*)![^ ]* NICK :(.*)"),
            };
            lClient.AddHandler(trigger, updateNamesOnNickChange, true);
        }

        public void joinChannelDefault(List<string> parameters, GroupCollection groups, bool receive)
        {
            foreach (string channel in Client.DefaultChannels)
            {
                joinChannel(channel);
            }
        }

        public void setupNamesOnJoin(List<string> parameters, GroupCollection groups, bool receive)
        {
            string capturedChannel = groups[1].Value;
            string capturedUsers = groups[2].Value;
            int index = Client.findChannelIndex(capturedChannel);
            if (index != -1)
            {
                Client.Channels.RemoveAt(index);
            }
            List<string> users = capturedUsers.Substring(1).Split(' ').ToList();
            Client.Channels.Add(new ChannelClass(Client, capturedChannel, users));
        }

        public void updateNamesOnJoin(List<string> parameters, GroupCollection groups, bool receive)
        {
            string capturedNick = groups[1].Value;
            string capturedIdent = groups[2].Value.ToLower();
            string capturedServer = groups[3].Value.ToLower();
            string capturedChannel = groups[4].Value;
            if (capturedNick.ToLower() != Client.NickLower)
            {
                int index = Client.findChannelIndex(capturedChannel);
                // Add the user to the channel user list
                Client.Channels[index].Users.Add(new ChannelUserClass(Client, capturedNick, capturedIdent, capturedServer));
            }
        }

        public void updateNamesOnPart(List<string> parameters, GroupCollection groups, bool receive)
        {
            string capturedNick = groups[1].Value;
            string capturedChannel = groups[2].Value;
            int channelIndex = Client.findChannelIndex(capturedChannel);
            if (capturedNick.ToLower() != Client.NickLower)
            {
                int index = Client.findChannelClientIndex(channelIndex, capturedNick);
                // Remove the user from the channel user list
                Client.Channels[channelIndex].Users.RemoveAt(index);
                // Find index
                int indexToSearch = Client.findUserIndex(capturedNick);
                // Remove the user from the global user list if user wasn't present in any other channel
                bool foundUsage = Client.inChannel(Client.Users[indexToSearch].ID);
                if (!foundUsage)
                {
                    Client.Users.RemoveAt(indexToSearch);
                }
            }
            else
            {
                List<ChannelUserClass> tempUsers = Client.Channels[channelIndex].Users;
                Client.Channels.RemoveAt(channelIndex);
                foreach (ChannelUserClass tempUser in tempUsers)
                {
                    int tempUserIndex = Client.findUserIndex(tempUser.ID);
                    // Remove the user from the global user list if user wasn't present in any other channel
                    if (Client.inChannel(tempUser.ID))
                    {
                        Client.Users.RemoveAt(tempUserIndex);
                    }
                }
            }
        }

        public void updateNamesOnQuit(List<string> parameters, GroupCollection groups, bool receive)
        {
            string capturedNick = groups[1].Value;
            if (capturedNick.ToLower() != Client.NickLower)
            {
                // Find ID
                int indexToSearch = Client.findUserIndex(capturedNick);
                // Remove the user from all channel user lists
                for (int x = 0; x < Client.Channels.Count(); x++)
                {
                    List<int> ToRemove = new List<int>();
                    for (int n = 0; n < Client.Channels[x].Users.Count(); n++)
                    {
                        if (Client.Channels[x].Users[n].ID == Client.Users[indexToSearch].ID)
                        {
                            ToRemove.Add(n);
                        }
                    }
                    foreach (int index in ToRemove)
                    {
                        Client.Channels[x].Users.RemoveAt(index);
                    }
                }
                // Remove the user from the global user list
                Client.Users.RemoveAt(indexToSearch);
            }
        }

        public void updateNamesOnNickChange(List<string> parameters, GroupCollection groups, bool receive)
        {
            string capturedNick = groups[1].Value;
            string capturedNewNick = groups[2].Value;
            int index = Client.findUserIndex(capturedNick);
            Client.Users[index].Name = capturedNewNick;
            Client.Users[index].NameLower = capturedNewNick.ToLower();
            if (capturedNick.ToLower() == Client.NickLower)
            {
                Client.Nick = capturedNewNick;
                Client.NickLower = capturedNewNick.ToLower();
            }
        }

        public void joinChannel(string channel)
        {
            Client.SendRaw("JOIN " + channel);
        }

        public void partChannel(string channel)
        {
            Client.SendRaw("PART " + channel);
        }
    }
}
