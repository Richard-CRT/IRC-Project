using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IRCLibrary;

namespace BotLibrary
{
    public class CoinflipCommand
    {
        public Random rnd = new Random();
        private Bot Bot;

        public CoinflipCommand(Bot bot)
        {
            bot.AddCommand(Status, false, "!COINFLIP", "!coinflip <count>", "Used to 'flip a coin' <count> times", 1, 1, 2);
            Bot = bot;
        }

        private void Status(UserClass user, ChannelClass channel, string message, List<string> messageList, int mediumType)
        {
            int count;
            bool valid = true;
            string output;
            if (messageList.Count == 2)
            {
                if (int.TryParse(messageList[1], out count))
                {
                    if (count > 0 & count <= 100)
                    {
                        output = GenerateString(count);
                        Send(user, channel, mediumType, output);
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
                output = GenerateString(1);
                Send(user, channel, mediumType, output);
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

        private string GenerateString(float count)
        {
            string output = "";
            float heads = 0;
            float tails = 0;
            List<char> Possibilities = new List<char>() { 'H', 'T' };
            for (int i=0;i< count;i++)
            {
                int r = rnd.Next(Possibilities.Count);
                switch (Possibilities[r])
                {
                    case 'H':
                        heads++;
                        break;
                    case 'T':
                        tails++;
                        break;
                }
                output = output + Possibilities[r];
            }
            output = output + " Heads: " + 100 * (heads / count) + "% Tails: " + 100 * (tails / count) + "%";
            return output;
        }
    }
}
