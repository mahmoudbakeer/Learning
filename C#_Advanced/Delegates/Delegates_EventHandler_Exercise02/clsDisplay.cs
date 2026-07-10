using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_EventHandler_Exercise02
{
    public class clsDisplay
    {
        public void Subscribe(clsBroker Broker)
        {
            Broker.Subscribe("TempChanged", TempratureDisplay);
        }
        public void TempratureDisplay(TempEventArgs e)
        {
            Console.WriteLine($"the temperature was {e.PrevTemp} and changed to {e.NewTemp}");
        }
    }
}
