using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_EventHandler_Exercise02
{
    public class TempEventArgs : EventArgs // the best practice 
    {
        public int PrevTemp { get; set; }
        public int NewTemp { get; set; }

        public TempEventArgs(int currentTemp, int newTemp)
        {
            PrevTemp = currentTemp;
            NewTemp = newTemp;
        }
    }
}
