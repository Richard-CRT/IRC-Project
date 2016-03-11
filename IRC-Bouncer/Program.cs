using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BouncerServerManagerNS;

namespace IRC_Bouncer
{
    class Program
    {
        static public BouncerManager bouncerManager { get; set; }

        static public void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            bouncerManager = new BouncerManager();

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
