using System.Net.Http.Headers;

namespace Threading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // the thread an independent path of execution within a program (process)
            // allowing multiple tasks to run concurrently. The System.Threading namespace provides the Thread class for creating and managing threads,
            // enabling improved application performance and responsiveness. 
            //Thread t = new Thread(Method1);
            //t.Start();


            //Thread t2 = new Thread(Method2);
            //t2.Start();

            //// they will run before the main , both concurrently
            //t.Join();
            //t2.Join();


            // use parametraized Method inside the thread done using Action delegate using lambda expression
            Thread t = new Thread(()=> Method1("Method 1"));
            t.Start();

            Thread t2 = new Thread(()=> Method1("Method 2"));
            t2.Start();


            for (int i = 0; i < 10; i++)
                Console.WriteLine($"Main : {i}");


        }
        public static void Method1()
        {
            
            for(int i = 0; i < 20; i++) 
                Console.WriteLine($"method1 : {i}");
        }
        public static void Method2()
        {

            for (int i = 0; i < 20; i++)
                Console.WriteLine($"method2 : {i}");
        }
        public static void Method1(string str)
        {

            for (int i = 0; i < 20; i++)
                Console.WriteLine($"{str} : {i}");
        }
        public static void Method2(string str)
        {

            for (int i = 0; i < 20; i++)
                Console.WriteLine($"{str} : {i}");
        }
    }
}
