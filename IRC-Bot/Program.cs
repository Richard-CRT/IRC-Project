﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BotLibrary;

namespace Chat_Bot
{
    class Program
    {
        static public Bot bot { get; set; }

        static public void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            bot = new Bot();
            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
