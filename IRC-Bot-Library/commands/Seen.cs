using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IRCLibrary.commands
{
    public class SeenCommand
    {
        private Bot Bot;
        public SeenCommand(Bot bot)
        {
            bot.AddCommand(Seen, false, "!SEEN", "!seen <nick>", "Used to display when the specified user last said something in the current channel", 2, 1, 0);
            Bot = bot;
        }

        private void Seen(UserClass user, ChannelClass channel, string message, List<string> messageList, int mediumType)
        {
            string channelName = channel.Name;
            if (messageList[1].ToLower() == Bot.Client.NickLower)
            {
                channel.SendMessage("It's meeeeeeeeeeeeee!!!");
            }
            else
            {
                string line = Bot.Client.logObject.findLastMessageUserChannel(messageList[1], channelName, new List<string> { "!seen" });
                if (line != null)
                {
                    List<string> lineList = line.Split(' ').ToList();
                    string targetUserNickWithSymbols = lineList[3].Split('!')[0];
                    string targetUserNick = targetUserNickWithSymbols.Substring(1, targetUserNickWithSymbols.Length - 1);
                    string totalString = "";
                    for (int i = 0; i < lineList.Count; i++)
                    {
                        if (i >= 6)
                        {
                            totalString = totalString + lineList[i] + " ";
                        }
                    }
                    bool action = false;
                    totalString = totalString.Substring(1, totalString.Length - 2); // remove : and extra space
                    if (Encoding.ASCII.GetBytes(totalString)[0] == 1)
                    {
                        action = true;
                    }
                    char terminateFormattingChar = '\u000F';
                    if (action)
                    {
                        totalString = targetUserNick + " " + totalString.Substring(8, totalString.Length - 9); // remove '0ACTION ' and '0'
                        totalString = totalString + terminateFormattingChar;
                        channel.SendMessage(targetUserNick + " was last seen saying *" + totalString + "* at " + lineList[1] + " " + Bot.Client.logObject.dateToReadableDate(lineList[0]) + " UTC in " + channel.Name + ".");
                    }
                    else
                    {
                        totalString = totalString + terminateFormattingChar;
                        channel.SendMessage(targetUserNick + " was last seen saying \"" + totalString + "\" at " + lineList[1] + " " + Bot.Client.logObject.dateToReadableDate(lineList[0]) + " UTC in " + channel.Name + ".");
                    }
                }
                else
                {
                    channel.SendMessage(messageList[1] + " has not said anything in " + channel.Name + " recorded in the log.");
                }
            }
        }
    }
}
