using System;
using System.Threading;
using System.Threading.Tasks;

// ==========================================
//  TASKS & MULTITHREADING (The Basics)
// ==========================================

class Program
{
    static void Main()
    {
        Console.WriteLine("Main thread started...");

        // 1. Task.Run: Grabs a background thread from the Thread Pool to execute the heavy work.
        // It does not block the main application thread from continuing.
        Task<int> task = Task.Run(() => GetPrimeInRange(2, 2_000_000));

        // ==========================================
        // HOW TO GET THE RESULT OF A TASK?
        // ==========================================

        // --- WAY 1: THE BAD WAY (Blocking) ---
        // Console.WriteLine($"Result: {task.Result}"); 
        // WHY IS IT BAD? 
        // It freezes (blocks) the current thread completely until the task is done. 
        // In a Web API, this destroys performance and causes Deadlocks. Never use it in modern apps.


        // --- WAY 2: THE LOW-LEVEL WAY (GetAwaiter) ---
        // var awaiter = task.GetAwaiter();
        // awaiter.OnCompleted(() => Console.WriteLine($"Result: {awaiter.GetResult()}"));
        // WHY WE AVOID IT? 
        // This is compiler-infrastructure code. It works, but it's not meant for everyday use.


        // --- WAY 3: THE CLASSIC GOOD WAY (ContinueWith) ---
        // This tells the task: "Whenever you finish, grab the result and execute this lambda".
        // It is excellent because it frees up the main thread to do other things in the meantime!
        //task.ContinueWith(t =>
        //{
        //    // 't' is the completed task.
        //    Console.WriteLine($"[ContinueWith] The result of the task is: {t.Result}");
        //});

        //// Prove that the main thread is NOT blocked!
        //Console.WriteLine("Main thread is free to do other work while primes are being calculated...");

        //Console.WriteLine("Main thread started...");
        //ShowThreadInfo(Thread.CurrentThread);

        // ==========================================
        // 1. SHORT-RUNNING TASK (Task.Run)
        // ==========================================
        // Use this for heavy calculations that EVENTUALLY FINISH.
        // It borrows a thread from the Thread Pool and returns it when done.
        //Console.WriteLine("\n--- Starting Short Task ---");
        //Task<int> task = Task.Run(() => GetPrimeInRange(2, 2_000_000));

        //task.ContinueWith(t =>
        //{
        //    Console.WriteLine($"[ContinueWith] Primes found: {t.Result}");
        //});


        // ==========================================
        // 2. LONG-RUNNING TASK (Task.Factory.StartNew)
        // ==========================================
        // HOW DO I KNOW IF I NEED IT?
        // If your task runs in an infinite loop (e.g., listening to a network port, 
        // processing a background queue constantly, or polling a database every second), 
        // it is a LONG-RUNNING task. 
        //
        // WHY NOT USE Task.Run?
        // Because it will permanently steal a thread from the Thread Pool (Thread Starvation),
        // causing your entire application to freeze and stop responding to new requests.
        //
        //  NOTE : You MUST pass TaskCreationOptions.LongRunning. This tells .NET: 
        // "Please create a brand NEW dedicated thread for this, do not touch the Thread Pool!"
        //Console.WriteLine("\n--- Starting Long Task ---");

        //Task longTask = Task.Factory.StartNew(
        //    () => RunLongTask(),
        //    TaskCreationOptions.LongRunning // <--- THE SECRET INGREDIENT!
        //);

        // one thing is good about tasks
        // the tasks not like thread the parent thread of task
        // means the creator of task such as the main thread
        // it will handel the exception happened even in the child task
        // lets give it a try
        try
        {
            Task.Run(() => ThrowException()).Wait();
            // if you did not add Wait() it won't be catched
            // because the creator 'parent' of the thread will not wait the return of the task 
            // and he will go for the Console.ReadLine() at the end
            // and he will be out of the try catch block when the exception happened
            // always use wait when the task have potential of returning exception
        }
        catch
        {
            Console.WriteLine("The exception is catched and handeled!!!");
        }

        // Just preventing the console from closing before the background tasks finish
        Console.ReadLine();
    }

    public static void ThrowException()
    {
        // any exception for the example
        throw new ArgumentNullException();
    }
    // A heavy, CPU-bound operation (Short-running)
    static int GetPrimeInRange(int from, int to)
    {
        // Prove that this is using the Thread Pool
        Console.Write("GetPrimeInRange Thread -> ");
        ShowThreadInfo(Thread.CurrentThread);

        if (from < 0 || to < 0) return 0;
        int count = 0;

        for (int i = from; i < to; i++)
        {
            if (i < 2) continue;
            int j = 2;
            bool isprime = true;

            while (j <= (int)Math.Sqrt(i))
            {
                if (i % j == 0)
                {
                    isprime = false;
                    break;
                }
                j++;
            }
            if (isprime) count++;
        }
        return count;
    }

    // A simulation of a long-running process (like listening to messages)
    public static void RunLongTask()
    {
        // Prove that this is NOT using the Thread Pool!
        Console.Write("RunLongTask Thread -> ");
        ShowThreadInfo(Thread.CurrentThread);

        // Simulating continuous background work
        Thread.Sleep(3000);
        Console.WriteLine("Long task completed its cycle!");
    }

    // A great helper method you wrote to see what's happening under the hood!
    public static void ShowThreadInfo(Thread th)
    {
        Console.WriteLine($"TID: {th.ManagedThreadId} | Pooled: {th.IsThreadPoolThread} | Background: {th.IsBackground}");
    }
}