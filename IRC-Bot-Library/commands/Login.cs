using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IRCLibrary;

namespace BotLibrary
{
    public class LoginCommand
    {
        private Bot Bot;

        public LoginCommand(Bot bot)
        {
            bot.AddCommand(Login, false, "!login", "!login <password>", "Used to authenticate user to a higher power", 2, 1, 1);
            Bot = bot;
        }

        public void Login(UserClass user, ChannelClass channel, string message, List<string> messageList, int mediumType)
        {
            string name = null;
            for (int i = 1; i < Bot.Passwords.Count(); i++)
            {
                if (Bot.Passwords[i].Item2 == messageList[1])
                {
                    user.Power = Bot.Passwords[i].Item3;
                    name = Bot.Passwords[i].Item1;
                }
            }
            if (name != null)
            {
                Bot.Client.respond(mediumType, channel, user, "", "Authenticated as " + name);
            }
        }
    }
}
