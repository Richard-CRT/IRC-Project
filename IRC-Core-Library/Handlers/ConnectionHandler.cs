using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IRCLibrary;

namespace IRCLibrary
{

    public class ConnectionHandler
    {
        private IRCUser Client;
        public ConnectionHandler(IRCUser lClient)
        {
            Client = lClient;
            List<Tuple<int, string>> trigger;
            // Register NICK and USER
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0, "SERVER:"),
                Tuple.Create(1, "CONNECTED")
            };
            lClient.AddHandler(trigger, register, false);

            // AUTH
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0, ":[^ ]*? 001 "+Regex.Escape(lClient.Nick)+" :.*"),
            };
            lClient.AddHandler(trigger, auth, true);

            // Respond to PING
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0,"PING"),
            };
            lClient.AddHandler(trigger, pong, false);
        }

        public void register(List<string> parameters, GroupCollection groups, bool receive)
        {
            Client.SendRaw("USER "+ Client.Ident +" "+Client.Hostname+" "+ Client.IP +" :"+ Client.Nick);
            Client.SendRaw("NICK "+ Client.Nick);
        }

        public void auth(List<string> parameters, GroupCollection groups, bool receive)
        {
            if (Client.AuthUsername != null)
            {
                Client.SendRaw("AUTH " + Client.AuthUsername + " " + Client.AuthPassword);
            }
            Client.SendRaw("MODE " + Client.Nick + " +x");
        }

        public void pong(List<string> parameters, GroupCollection groups, bool receive)
        {
            Client.SendRaw("PONG " + parameters[1]);
        }
    }
}
