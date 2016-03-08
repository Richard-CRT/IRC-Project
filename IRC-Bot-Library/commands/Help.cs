using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IRCLibrary;

namespace IRCLibrary.handlers
{
    public class HelpCommand
    {
        private Bot Bot;

        public HelpCommand(Bot bot)
        {
            bot.AddCommand(Help, false, "!HELP", "!help <command ID>", "Displays list of commands the bot can respond to or information on a specific command if ID given from main help list", 1, 1, 1);
            Bot = bot;
        }

        public void Help(UserClass user, ChannelClass channel, string message, List<string> messageList, int mediumType)
        {
            int index;
            bool valid = true;
            if (messageList.Count == 2)
            {
                if (int.TryParse(messageList[1], out index))
                {
                    index--;
                    if (index >= 0 & index <= Bot.CommandObservers.Count - 1)
                    {
                        CommandObserverClass command = Bot.CommandObservers[index];

                        string mediumString = "";
                        switch (command.MediumType)
                        {
                            case 0:
                                mediumString = "Channel";
                                break;
                            case 1:
                                mediumString = "Private Message";
                                break;
                            case 2:
                                mediumString = "Channel or Private Message";
                                break;
                        }
                        string authenticationString = "";
                        foreach (Tuple<string, string, int> password in Bot.Passwords)
                        {
                            if (command.RequiredPower == password.Item3)
                            {
                                authenticationString = password.Item1;
                            }
                        }
                        user.SendMessage("Command:");
                        user.SendMessage(command.Usage);
                        user.SendMessage(command.Description);
                        user.SendMessage("Allowed Medium(s): " + mediumString);
                        user.SendMessage("Minimum Required Authentication: " + authenticationString);
                    }
                    else
                    {
                        valid = false;
                    }
                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }
            if (!valid)
            {
                user.SendMessage("Commands:");
                for (int i = 0; i < Bot.CommandObservers.Count; i++)
                {
                    CommandObserverClass command = Bot.CommandObservers[i];
                    user.SendMessage((i + 1) + ". " + command.Usage);
                }
            }
        }
    }
}
