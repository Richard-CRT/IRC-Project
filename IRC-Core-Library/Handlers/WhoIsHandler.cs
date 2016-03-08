using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IRCLibrary;

namespace IRCLibrary.handlers
{
    public class WhoIsHandler
    {
        private IRCUser Client;
        public WhoIsHandler(IRCUser lClient)
        {
            Client = lClient;
            List<Tuple<int, string>> trigger;

            // Update ident and server on WHOIS results
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0,".*?352.*?"+Regex.Escape(lClient.Nick)+" [^ ]* ([^ ]*) ([^ ]*) [^ ]* ([^ ]*).*"),
            };
            lClient.AddHandler(trigger, updateNameOnWHOIS, true);
        }

        public void updateNameOnWHOIS(List<string> parameters, GroupCollection groups, bool receive)
        {
            string capturedIdent = groups[1].Value.TrimStart(new char[] { '~' });
            string capturedServer = groups[2].Value;
            string capturedNick = groups[3].Value;
            int index = Client.findUserIndex(capturedNick);
            if (index != -1)
            {
                Client.Users[index].Ident = capturedIdent;
                Client.Users[index].Server = capturedServer;
            }
        }
    }
}
