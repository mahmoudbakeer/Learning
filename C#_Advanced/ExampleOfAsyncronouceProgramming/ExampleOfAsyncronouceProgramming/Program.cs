namespace ExampleOfAsyncronouceProgramming
{


    public class CustomEventArga : EventArgs
    {
        public int parameter1 { get;  }
        public string parameter2 { get; }
        public CustomEventArga(int parameter1 , string parameter2)
        {
            this.parameter1 = parameter1;
            this.parameter2 = parameter2;
        }
    }
    internal class Program
    {
        public static EventHandler<CustomEventArga> CallBackEventHandler;
        static async Task Main(string[] args)
        {
            CallBackEventHandler += OnTaskCompleted;
            Console.WriteLine("Doing some other work .....");
            await AsyncSimulator(CallBackEventHandler);


            Console.WriteLine("EveryThing Done ....");
        }

        // You always use async when you want to perform await

        public static async Task AsyncSimulator(EventHandler<CustomEventArga> mycallbackFunc)
        {
            await Task.Delay(2000);
            mycallbackFunc?.Invoke(null , new CustomEventArga(20 , "Syria UndertheWater...."));
        }

        public static void OnTaskCompleted(object sender , CustomEventArga e)
        {
            Console.WriteLine($"The first parameter is = {e.parameter1} ,the second parameter is :  {e.parameter2}");
        }
    }
}
