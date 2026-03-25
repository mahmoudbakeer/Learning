using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_EventHandler_Exercise02
{
    public class clsAlarm
    {

        private int Alarmthreshold {  get; set; }
        public clsAlarm(int value)
        {
            Alarmthreshold = value;
        }
        public void SetAlarmThreshold(int value)
        {
            Alarmthreshold = value;
        }   
        public void Subscribe(clsBroker Broker)
        {
            Broker.Subscribe("TempChanged", FireAlarm);
        }
        public void FireAlarm(TempEventArgs e)
        {
            if (e.NewTemp > Alarmthreshold) Console.WriteLine($"Take care its to hot and the temprature exceeded the threshold {Alarmthreshold}");
        }
    }
}
