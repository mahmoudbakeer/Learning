using System.Net;

namespace TaskLibrary
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Task<int> result = TaskSimulate();
            // non blocking task 
            Console.WriteLine("DO SOME OTHER WORK......");
            int res = await result; // wait the result to complete its task
            Console.WriteLine($"The result of the first Task is {res}");
        }
        public static async Task<int> TaskSimulate()
        {
            // delay the task 2 seconds to simulate idle time 
            await Task.Delay(2000);
            return 10;
        }
        
    }
}
