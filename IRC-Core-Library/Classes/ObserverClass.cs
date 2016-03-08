using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCLibrary
{
    public class ObserverClass
    {
        public List<Tuple<int,string>> Trigger { get; set; }
        public MethodHandlerCallBack Callback { get; set; }
        public bool Regex { get; set; }

        public ObserverClass(List<Tuple<int, string>> trigger, MethodHandlerCallBack callback, bool regex)
        {
            Trigger = trigger;
            Callback = callback;
            Regex = regex;
        }
    }
}
