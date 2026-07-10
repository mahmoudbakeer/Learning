using System;
using System.Threading;
using System.Threading.Tasks;

// ==========================================
// ASYNC & AWAIT (The Deep Dive & The "Freezing" Trap)
// The modern, clean way to handle Asynchronous operations without blocking threads.
// ==========================================

// Make Main async so we can use 'await' inside it
//Console.WriteLine("1. Caller started. Requesting work...");

// ==========================================
//  STEP 1: FIRE THE TASK IN THE BACKGROUND
// ==========================================
// Call the async function. 
// We DO NOT use 'await' here yet, so it runs in the background.
//Task<string> messageTask = GetWorkAsync();

// ==========================================
//  STEP 2: CONCURRENT WORK (Before Await)
// ==========================================
// GOLDEN RULE PART 1: 
// Any code ABOVE the 'await' runs concurrently with the background task!
// The Main thread is completely FREE to do other work right now.
//Console.WriteLine("2. Main thread is free! Printing this WHILE the task runs...");
//Console.WriteLine("   -> Doing math: 5 + 5 = " + (5 + 5));

// ==========================================
//  STEP 3: THE 'AWAIT' MAGIC (Yielding, not Blocking)
// ==========================================
// Now we actually need the result to continue. 
// 
// THE DIFFERENCE BETWEEN METHOD PAUSE & THREAD BLOCK:
// When the compiler hits 'await', it PAUSES this specific method (Main), 
// BUT it FREES the Thread! The thread goes back to the Thread Pool to do other things.
// Think of the Thread as a cashier: they send your order to the kitchen (Task), 
// give you a pager, and go serve the next customer instead of freezing in place.

//string finalMessage = await messageTask;

// ==========================================
//  STEP 4: CONTINUATION (After Await)
// ==========================================
// GOLDEN RULE PART 2: 
// Any code BELOW the 'await' is a "Continuation". It will NEVER run 
// until the background task is 100% finished.
// That is why "Check the freezing" didn't print immediately in your first test!

//Console.WriteLine($"3. Result received: {finalMessage}");
//Console.WriteLine("4. Check the freezing... (This prints LAST because it was below the await!)");


// now here test the cancellation token 
// please let me know which approach is the best of handeling the cancellation token
//CancellationTokenSource cts = new CancellationTokenSource();

// await DoCheck(cts);
//await DoCheck2(cts);
//await DoCheck3(cts);

// I added the 4th "Best" approach here to test:
//await DoCheckBest(cts);

Console.WriteLine("--- Testing Concurrency ---");
Console.WriteLine("Starting both YouTube requirements concurrently...\n");

// 1. Start both tasks immediately. 
// Because they use 'await Task.Delay' inside, they yield the thread instantly.
// Both timers (4s and 3s) are now ticking down AT THE SAME TIME!
Task t1 = Has4000ViewHours();
Task t2 = Has1000Subscriber();

// ==========================================
// A. Task.WhenAny (The Race)
// ==========================================
// Pauses until the FIRST task finishes. It ignores the rest.
//Console.WriteLine("[System] Waiting for the FIRST goal to be reached...");

//Task firstCompletedTask = await Task.WhenAny(t1, t2);

//Console.WriteLine($"\n[System] A goal was reached! (WhenAny triggered)");


// ==========================================
// B. Task.WhenAll (The Checkpoint)
// ==========================================
// Pauses until ALL tasks in the list have finished.
Console.WriteLine("\n[System] Now waiting for ALL goals to be reached for monetization...");

await Task.WhenAll(t1, t2);

Console.WriteLine("\n[System] All YouTube requirements met! (WhenAll triggered)");

Console.ReadLine();

// ==========================================
// STEPS TO MAKE AN ASYNCHRONOUS FUNCTION:
// 1. Postfix the name with "Async" (Naming Convention).
// 2. Add the 'async' keyword in the signature.
// 3. Return 'Task' (for void) or 'Task<DataType>' (for a return value).
// 4. Use 'await' inside the body on I/O bound operations.
// ==========================================
static async Task<string> GetWorkAsync()
{
    Console.WriteLine("\t[GetWorkAsync] Started. Calling GetStringAsync...");

    // 'await' unwraps the Task<string> and gives us the actual string!
    string result = await GetStringAsync();

    Console.WriteLine("\t[GetWorkAsync] Finished waiting.");
    return result;
}

static async Task<string> GetStringAsync()
{
    // We MUST use 'await' here, otherwise the delay is ignored!
    // Task.Delay simulates a true Asynchronous I/O operation (like calling an API or DB).
    await Task.Delay(2000);

    return "Task Completed successfully!";
}

static async Task DoCheck(CancellationTokenSource cancellationTokenSource)
{
    // lets create a task to check if the process was cancelled
    // we won't put await here because we don't want it to block the thread from doing the other things
    Task.Run(() =>
    {
        var input = Console.ReadKey();
        if (input.Key == ConsoleKey.Q)
        {
            cancellationTokenSource.Cancel();
        }
    });

    // here we make infinite loop to simultae expensive operation
    while (true)
    {
        cancellationTokenSource.Token.ThrowIfCancellationRequested();
        Console.WriteLine("Check...");
        await Task.Delay(4000);
        Console.Write($"Completed on {DateTime.Now}");
        Console.WriteLine();
    }
    cancellationTokenSource.Dispose();
    Console.WriteLine("This operation was terminated.");
}

static async Task DoCheck2(CancellationTokenSource cancellationTokenSource)
{
    // lets create a task to check if the process was cancelled
    // we won't put await here because we don't want it to block the thread from doing the other things
    Task.Run(() =>
    {
        var input = Console.ReadKey();
        if (input.Key == ConsoleKey.Q)
        {
            cancellationTokenSource.Cancel();
        }
    });

    // we will do the same thing but we will send the cancellationtoken to the Delay method
    while (true)
    {
        Console.Write("Check...");
        await Task.Delay(4000, cancellationTokenSource.Token);
        Console.WriteLine($"Completed on {DateTime.Now}");
        Console.WriteLine();
    }
    cancellationTokenSource.Dispose();
    Console.WriteLine("This operation was terminated.");
}

static async Task DoCheck3(CancellationTokenSource cancellationTokenSource)
{
    // lets create a task to check if the process was cancelled
    // we won't put await here because we don't want it to block the thread from doing the other things
    Task.Run(() =>
    {
        var input = Console.ReadKey();
        if (input.Key == ConsoleKey.Q)
        {
            cancellationTokenSource.Cancel();
        }
    });

    // now we will do one good thing here is to catch the exception
    try
    {
        while (true)
        {
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            Console.Write("Check...");
            await Task.Delay(4000);
            Console.WriteLine($"Completed on {DateTime.Now}");
            Console.WriteLine();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    cancellationTokenSource.Dispose();
    Console.WriteLine("This operation was terminated.");
}

// ==========================================
// 🏆 THE BEST APPROACH (Industry Standard)
// Added to combine the best parts of your checks!
// Combines immediate cancellation with safe exception handling.
// ==========================================
static async Task DoCheckBest(CancellationTokenSource cancellationTokenSource)
{
    // Background task listening for the 'Q' key to trigger cancellation
    Task.Run(() =>
    {
        var input = Console.ReadKey(true);
        if (input.Key == ConsoleKey.Q)
        {
            Console.WriteLine("\n[System] Cancellation Requested by User!");
            cancellationTokenSource.Cancel();
        }
    });

    try
    {
        while (true)
        {
            // 1. Manually check if doing heavy CPU work before proceeding
            cancellationTokenSource.Token.ThrowIfCancellationRequested();

            Console.Write("Check (Best)...");

            // 2. CRITICAL: Pass the token to the async method so it wakes up instantly!
            await Task.Delay(4000, cancellationTokenSource.Token);

            Console.WriteLine($" Completed on {DateTime.Now}");
        }
    }
    // 3. Catch the specific cancellation exception cleanly
    catch (OperationCanceledException)
    {
        Console.WriteLine("\n[Handled] The operation was safely aborted without crashing.");
    }
    finally
    {
        // 4. ALWAYS dispose of the CancellationTokenSource in a finally block
        cancellationTokenSource.Dispose();
        Console.WriteLine("Clean up complete. This operation was terminated.");
    }
}


// now lets test the whenall and whenany in tasks
static Task Has4000ViewHours()
{
    Task.Delay(4000).Wait();
    return Task.Run(() =>
    {
        Console.WriteLine($"Gongratulations you now have 4000 hours views...");
    });
}
static Task Has1000Subscriber()
{
    Task.Delay(3000).Wait(); // lets make it less to test the any
    return Task.Run(() =>
    {
        Console.WriteLine($"Gongratulations you now have 1000 subcribers...");
    });
}