using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using IRCLibrary;
using IRCLibrary.handlers;

namespace IRCLibrary
{
    public delegate void IRCMessageCallBack(string result, bool receive);
    public delegate void MethodHandlerCallBack(List<string> result, GroupCollection groups, bool receive);

    public class IRCUser
    {
        private List<ObserverClass> Observers { get; set; } = new List<ObserverClass>();
        public List<ChannelClass> Channels { get; set; } = new List<ChannelClass>();
        public ChannelHandler channelHandler;
        public ConnectionHandler connectionHandler;
        public WhoIsHandler whoIsHandler;
        public LogObject logObject;
        public char[] userPrefixes = new char[0];

        public string Nick { get; set; }
        public string NickLower { get; set; }
        public string IP { get; set; }
        public string Ident { get; set; }
        public string Hostname { get; set; }
        public string AuthUsername { get; set; }
        public string AuthPassword { get; set; }
        public int Port { get; set; }
        public AsynchronousClient SocketConnection { get; set; }
        public List<string> DefaultChannels { get; set; }
        public List<UserClass> Users = new List<UserClass>();

        public IRCUser(string nick, string ip, int port, List<string> channels, string ident, string hostname, string authUsername, string authPassword)
        {
            Nick = nick;
            NickLower = nick.ToLower();
            IP = ip;
            Port = port;
            Ident = ident;
            Hostname = hostname;
            AuthUsername = authUsername;
            AuthPassword = authPassword;
            DefaultChannels = channels;

            //

            channelHandler = new ChannelHandler(this);
            connectionHandler = new ConnectionHandler(this);
            whoIsHandler = new WhoIsHandler(this);
            logObject = new LogObject(this);

            List<Tuple<int, string>> trigger;

            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0,".*?005.*?"+Regex.Escape(Nick)+".*?PREFIX=\\(.*?\\)([^ ]*).*"),
            };
            AddHandler(trigger, updatePrefix, true);
        }

        public void CommandHandler(string message, bool receive)
        {
            message = message.TrimEnd(new char[] { '\r' , '\n' });
            if (receive)
            {
                Print(Nick + " ← " + message);
            } else
            {
                Print(Nick + " → " + message);
            }
            logObject.log(message, receive);
            foreach (ObserverClass observer in Observers)
            {
                List<string> parameters = message.Split(' ').ToList();
                if (observer.Regex)
                {
                    RegexOptions options = RegexOptions.IgnoreCase;
                    Regex rgx = new Regex(@observer.Trigger[0].Item2, options);
                    Match groups = rgx.Match(message);
                    if (rgx.IsMatch(message))
                    {
                        //Console.WriteLine(observer.Callback.Target.ToString() + " : "+observer.Callback.Method.ToString());
                        observer.Callback(parameters,groups.Groups,receive);
                    }
                }
                else
                {
                    if (observer.Trigger.Count() <= parameters.Count())
                    {
                        bool difference = false;
                        foreach (Tuple<int, string> triggerParam in observer.Trigger)
                        {
                            if (triggerParam.Item2 != parameters[triggerParam.Item1])
                            {
                                difference = true;
                            }
                        }
                        if (!difference)
                        {
                            //Console.WriteLine(observer.Callback.Target.ToString() + " : " + observer.Callback.Method.ToString());
                            observer.Callback(parameters, null, receive);
                        }
                    }
                }
            }
            return;
        }

        public void updatePrefix(List<string> parameters, GroupCollection groups, bool receive)
        {
            string capturedPrefixes = groups[1].Value;
            userPrefixes = capturedPrefixes.ToCharArray();
        }

        public void Connect()
        {
            IRCMessageCallBack callback = CommandHandler;
            SocketConnection = new AsynchronousClient(callback, IP, Port);
            SocketConnection.Start();
        }

        public void AddHandler(List<Tuple<int, string>> trigger, MethodHandlerCallBack callback, bool regex)
        {
            Observers.Add(new ObserverClass(trigger, callback, regex));
        }

        public void RemoveHandler(List<Tuple<int, string>> trigger, MethodHandlerCallBack callback, bool regex)
        {
            Observers.Remove(new ObserverClass(trigger, callback, regex));
        }

        public void Print(string line)
        {
            Console.WriteLine(line);
        }

        public void SendRaw(string message)
        {
            SocketConnection.Send(message+"\n");
        }

        public void Shutdown()
        {
            Channels = new List<ChannelClass>();
            Users = new List<UserClass>();
        }

        public int findUserIndex(string user)
        {
            int indexToSearch = -1;
            for (int i = 0; i < Users.Count(); i++)
            {
                if (Users[i].NameLower == user.ToLower())
                {
                    indexToSearch = i;
                    break;
                }
            }
            return indexToSearch;
        }

        public int findUserIndex(int id)
        {
            int indexToSearch = -1;
            for (int i = 0; i < Users.Count(); i++)
            {
                if (Users[i].ID == id)
                {
                    indexToSearch = i;
                    break;
                }
            }
            return indexToSearch;
        }

        public int findChannelClientIndex(int channelIndex, string user)
        {
            user = user.ToLower();
            int index = -1;
            for (int i = 0; i < Channels[channelIndex].Users.Count(); i++)
            {
                if (Channels[channelIndex].Users[i].NameLower == user)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public int findChannelIndex(string channel)
        {
            int index = -1;
            for (int i = 0; i < Channels.Count(); i++)
            {
                if (Channels[i].NameLower == channel.ToLower())
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public bool inChannel(int id)
        {
            int indexToSearch = findUserIndex(id);
            bool foundUsage = false;
            for (int x = 0; x < Channels.Count(); x++)
            {
                for (int n = 0; n < Channels[x].Users.Count(); n++)
                {
                    if (Channels[x].Users[n].ID == Users[indexToSearch].ID)
                    {
                        foundUsage = true;
                        break;
                    }
                }
                if (foundUsage)
                {
                    break;
                }
            }
            return foundUsage;
        }

        public bool userInChannel(UserClass user, ChannelClass channel)
        {
            bool present = false;
            for (int i = 0; i < channel.Users.Count(); i++)
            {
                if (channel.Users[i].ID == user.ID)
                {
                    present = true;
                    break;
                }
            }
            return present;
        }

        public void respond(int mediumType, ChannelClass channel, UserClass user, string channelMessage, string userMessage)
        {
            if (mediumType == 0)
            {
                channel.SendMessage(channelMessage);
            }
            else if (mediumType == 1)
            {
                user.SendMessage(userMessage);
            }
        }
    }
}
