using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IRCLibrary;

namespace BotLibrary
{

    public class BotUserCommandHandler
    {
        private Bot Bot;

        public BotUserCommandHandler(Bot bot)
        {
            Bot = bot;
            List<Tuple<int, string>> trigger;

            // Channel command handling
            trigger = new List<Tuple<int, string>>
            {
                Tuple.Create(0,":([^ ]*?)![~]*?.*?@.*? PRIVMSG ([^ ]*) :(.*)"),
            };
            Bot.Client.AddHandler(trigger, command, true);

            SeenCommand seen = new SeenCommand(bot);
            LoginCommand login = new LoginCommand(bot);
            StatusCommand status = new StatusCommand(bot);
            HelpCommand help = new HelpCommand(bot);
            IRCCommand irc = new IRCCommand(bot);
        }

        public void command(List<string> parameters, GroupCollection groups, bool receive)
        {
            string capturedNick = groups[1].Value;
            string capturedChannel = groups[2].Value;
            string message = groups[3].Value;
            int channelIndex = Bot.Client.findChannelIndex(capturedChannel);
            ChannelClass channel;
            if (channelIndex == -1)
            {
                channel = null;
            }
            else
            {
                channel = Bot.Client.Channels[channelIndex];
            }
            int mediumType;
            if (channel == null)
            {
                mediumType = 1;
            }
            else
            {
                mediumType = 0;
            }
            int userIndex = Bot.Client.findUserIndex(capturedNick);
            UserClass user = Bot.Client.Users[userIndex];
            if (user.NameLower != Bot.Client.NickLower)
            {
                List<string> messageList = message.Split(' ').ToList();
                foreach (CommandObserverClass commandObserver in Bot.CommandObservers)
                {
                    if (commandObserver.MediumType == 2 | (commandObserver.MediumType == mediumType) | (commandObserver.MediumType == mediumType))
                    {
                        if ((user.Power >= commandObserver.RequiredPower & messageList.Count >= commandObserver.MinimumParams & messageList.Count >= 1 & commandObserver.Command == messageList[0].ToUpper()) | commandObserver.MatchAll)
                        {
                            commandObserver.Callback(user, channel, message, messageList, mediumType);
                        }
                    }
                }
            }
        }
    }
}
