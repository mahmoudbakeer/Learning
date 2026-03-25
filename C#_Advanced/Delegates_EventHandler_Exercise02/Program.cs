using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_EventHandler_Exercise02
{
    internal class Program
    {
        static void Main(string[] args)
        {


            /*
                EventHandler is a generic delegate such as Action<EventArgs> has no return type and also give the event protection 
                where user you can't assign new value to the Invocation list and the event can't be called directley from outer spaces
                this system represents the EventHandler use in multiple cases
                demonstrating the work of it and exercising on the observer pattern in SOLID principles
             */
            clsSensor sensor = new clsSensor();
            clsAlarm alarm = new clsAlarm(20); // initial value
            clsDisplay display = new clsDisplay();

            // subscribtion in Event Handler
            sensor.SensorChanged += alarm.FireAlarm;
            sensor.SensorChanged += display.TempratureDisplay;


            while (true)
            {
                Console.WriteLine("Welcome to the temperature system");
                Console.WriteLine("1. to set the sensor temperature");
                Console.WriteLine("2. to set the alarm threshold temperature");
                Console.WriteLine("3. to Exit the system");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Please Enter the new temprature :");

                        int Sensorvalue = Convert.ToInt32(Console.ReadLine());
                        sensor.SetSensorValue(Sensorvalue);
                        break;
                    case "2":
                        Console.WriteLine("Please Enter the new threshold :");

                        int Alarmthreshold = Convert.ToInt32(Console.ReadLine());
                        alarm.SetAlarmThreshold(Alarmthreshold);
                        break;
                    case "3": return;
                    default: 
                        Console.WriteLine("Wrong input please enter again!");
                        break;
                }


            }
        }
    }
}
