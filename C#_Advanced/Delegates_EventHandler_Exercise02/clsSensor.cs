using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_EventHandler_Exercise02
{
    public class clsSensor
    {
        public event EventHandler<TempEventArgs> SensorChanged; // the event delegate 
        private int SensorTemp {  get; set; }
        public void SetSensorValue(int value)
        {
            // the call of the delegate
            SensorChanged?.Invoke(this, new TempEventArgs(SensorTemp , value));
            SensorTemp = value;
        }
    }
}
