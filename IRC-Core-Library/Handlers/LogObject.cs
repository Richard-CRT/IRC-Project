using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using IRCLibrary;

namespace IRCLibrary.handlers
{
    public class LogObject
    {
        private IRCUser Client;
        private StreamWriter LogFile;
        private string LogDirectory;
        private string OldFileName;
        private System.Text.UTF8Encoding utf8WithoutBom = new System.Text.UTF8Encoding(false);

        public LogObject(IRCUser lClient, bool debug)
        {
            bool exists = System.IO.Directory.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "logs"));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "logs"));
            }
            LogDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "logs");

            if (!debug)
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                string tempName = generateName();
                LogFile = new StreamWriter(Path.Combine(LogDirectory, tempName), true, utf8WithoutBom);
                LogFile.AutoFlush = true;
                OldFileName = tempName;
            }
            Client = lClient;
        }

        private string generateName()
        {
            return DateTime.Now.ToString("yyyyMMdd")+".log";
        }
        
        public void log(string line, bool receive)
        {
            string tempName = generateName();
            if (OldFileName != tempName)
            {
                LogFile.Close();
                LogFile = new StreamWriter(Path.Combine(LogDirectory, tempName), true, utf8WithoutBom);
                LogFile.AutoFlush = true;
                OldFileName = tempName;
            }
            string timedate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (receive)
            {
                LogFile.WriteLine(timedate + " RECEIVED " + line);
            }
            else
            {
                LogFile.WriteLine(timedate + " SENT " + line);
            }
        }

        public string dateToReadableDate(string date)
        {
            List<string> dateList = date.Split('-').ToList();
            return dateList[2] + "/" + dateList[1] + "/" + dateList[0];
        }

        public List<string> getLogFiles()
        {
            List<string> allFiles = Directory.GetFiles(LogDirectory).ToList();
            List<string> logFilesList = new List<string>();
            for (int i = 0; i < allFiles.Count(); i++)
            {
                if (allFiles[i].Length == 12 + (LogDirectory.Length + 1))
                {
                    if (allFiles[i].Substring((LogDirectory.Length + 1) + 8).ToUpper() == ".LOG")
                    {
                        logFilesList.Add(allFiles[i]);
                    }
                }
            }
            logFilesList.Sort();
            return logFilesList;
        }

        public string findLastRegex(string regex, char splitChar, int parameter, string firstStringNotAllowed)
        {
            List<string> logFilesList = getLogFiles();
            logFilesList.Reverse();
            List<string> totalLines = new List<string>();
            for (int i = 0; i < logFilesList.Count(); i++)
            {
                List<string> tempLines;
                if (OldFileName != logFilesList[i].Substring((LogDirectory.Length + 1)))
                {
                    tempLines = File.ReadAllLines(logFilesList[i]).ToList();
                }
                else
                {
                    LogFile.Close();
                    tempLines = File.ReadAllLines(logFilesList[i]).ToList();
                    LogFile = new StreamWriter(Path.Combine(LogDirectory, OldFileName), true, utf8WithoutBom);
                    LogFile.AutoFlush = true;
                }
                tempLines.Reverse();
                for (int x = 0; x < tempLines.Count; x++)
                {
                    RegexOptions options = RegexOptions.IgnoreCase;
                    Regex rgx = new Regex(@regex, options);
                    if (rgx.IsMatch(tempLines[x]) | regex == null)
                    {
                        List<string> split = tempLines[x].Split(splitChar).ToList();
                        if (split.Count > parameter)
                        {
                            if (split[parameter].Length >= firstStringNotAllowed.Length+1)
                            {
                                if (split[parameter].Substring(1, firstStringNotAllowed.Length) != firstStringNotAllowed)
                                {
                                    return tempLines[x];
                                }
                            }
                            else
                            {
                                return tempLines[x];
                            }
                        }
                    }
                }
            }
            return null;
        }

        public string findLastMessageUserChannel(string name, string channel, List<string> forbiddenStarts)
        {
            List<string> logFilesList = getLogFiles();
            logFilesList.Reverse();
            List<string> totalLines = new List<string>();
            for (int i = 0; i < logFilesList.Count(); i++)
            {
                List<string> tempLines;
                if (OldFileName != logFilesList[i].Substring((LogDirectory.Length + 1)))
                {
                    tempLines = File.ReadAllLines(logFilesList[i]).ToList();
                }
                else
                {
                    LogFile.Close();
                    tempLines = File.ReadAllLines(logFilesList[i]).ToList();
                    LogFile = new StreamWriter(Path.Combine(LogDirectory, OldFileName), true, utf8WithoutBom);
                    LogFile.AutoFlush = true;
                }
                tempLines.Reverse();
                for (int x = 0; x < tempLines.Count; x++)
                {
                    string line = tempLines[x];
                    List<string> lineSplit = line.Split(' ').ToList();
                    if (lineSplit.Count >= 7)
                    {
                        if (lineSplit[3].Length >= name.Length+1)
                        {
                            if (lineSplit[2] == "RECEIVED" & lineSplit[4] == "PRIVMSG" & lineSplit[5] == channel & lineSplit[3].Substring(1, name.Length) == name & !forbiddenStarts.Contains(lineSplit[6].Substring(1)))
                            {
                                return line;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
