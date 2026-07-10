using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_EventHandler_Exercise02
{
    public class clsBroker
    {

        private Dictionary<string, List<Action<TempEventArgs>>> _SubscribtionList = new Dictionary<string, List<Action<TempEventArgs>>>();


        public void Subscribe(string eventName, Action<TempEventArgs> method)
        {
            if (_SubscribtionList.ContainsKey(eventName))
            {
                _SubscribtionList[eventName].Add(method);
            }
            else
            {
                _SubscribtionList.Add(eventName, new List<Action<TempEventArgs>>() { method });
            }
        }
        public void Publish(string eventName, TempEventArgs args)
        {
            if (_SubscribtionList.ContainsKey(eventName))
            {
                foreach (var method in _SubscribtionList[eventName])
                {
                    method(args);
                }
            }
        }

    }
}
