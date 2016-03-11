using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IRCLibrary;

namespace BotLibrary
{
    public class StatusCommand
    {
        private Bot Bot;
        public StatusCommand(Bot bot)
        {
            bot.AddCommand(Status, false, "!STATUS", "!status", "Used to display the Minion's running status", 1, 1, 2);
            Bot = bot;
        }

        private void Status(UserClass user, ChannelClass channel, string message, List<string> messageList, int mediumType)
        {
            List<string> lines = new List<string>();
            lines.Add("Status");
            lines.Add("- Channels: "+Bot.Client.Channels.Count);
            lines.Add("- Commands: "+Bot.CommandObservers.Count);
            foreach (string line in lines)
            {
                Send(user, channel, mediumType, line);
            }
        }

        private void Send(UserClass user, ChannelClass channel, int mediumType, string message)
        {
            if (mediumType == 0)
            {
                channel.SendMessage(message);
            }
            else if (mediumType == 1)
            {
                user.SendMessage(message);
            }
        }
    }
}
