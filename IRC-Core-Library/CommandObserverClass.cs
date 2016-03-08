using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRCLibrary.handlers;

namespace IRCLibrary
{
    public delegate void CommandHandlerCallBack(UserClass user, ChannelClass channel, string message, List<string> messageList, int mediumType);

    public class CommandObserverClass
    {
        public string Command;
        public string Usage;
        public string Description;
        public bool MatchAll;
        public int MinimumParams;
        public int RequiredPower;
        public int MediumType;
        public CommandHandlerCallBack Callback { get; set; }

        public CommandObserverClass(CommandHandlerCallBack callBack, bool matchAll, string command, string usage, string description, int minimumParams, int requiredPower, int mediumType)
        {
            Command = command.ToUpper();
            Usage = usage;
            Description = description;
            MatchAll = matchAll;
            MinimumParams = minimumParams;
            RequiredPower = requiredPower;
            MediumType = mediumType;
            Callback = callBack;
        }
    }
}
