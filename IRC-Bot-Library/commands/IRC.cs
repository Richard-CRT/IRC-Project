using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IRCLibrary;

namespace BotLibrary
{
    public class IRCCommand
    {
        private Bot Bot;

        public IRCCommand(Bot bot)
        {
            bot.AddCommand(Irc, false, "!IRC", "!irc <command>", "Executes IRC command", 2, 3, 2);
            Bot = bot;
        }

        public void Irc(UserClass user, ChannelClass channel, string message, List<string> messageList, int mediumType)
        {
            List<string> tempList = messageList;
            tempList.RemoveAt(0);
            string command = String.Join(" ", tempList);
            Bot.Client.SendRaw(command);
        }
    }
}
