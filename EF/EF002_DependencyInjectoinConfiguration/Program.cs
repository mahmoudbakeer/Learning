using EF002_DependenctyInjectionConfiguratoin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

// ==========================================
// EF CORE: DEPENDENCY INJECTION & CRUD
// ==========================================

// --- WHY USE DI WITH EXTERNAL CONFIGURATION? ---
// 1. Loose Coupling: Your methods and classes no longer need to know HOW to create a DbContext 
//    or what options it needs. They just ask the "DI Container" for a ready-to-use instance.
// 2. Lifecycle Management: The DI Container is incredibly smart. It manages when to create the 
//    DbContext and when to destroy it (Dispose) efficiently, saving server memory.
// 3. Testability: When writing Unit Tests, you can easily trick the DI Container into providing 
//    an In-Memory database instead of a real SQL Server, without changing your core logic.

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string connectionString = configuration.GetConnectionString("DefaultConnection")
                          ?? configuration.GetSection("constr").Value!;

// ==========================================
// DEPENDENCY INJECTION SETUP
// ==========================================

// --- WHAT IS ServiceCollection AND WHY USE IT? ---
// Think of ServiceCollection as a "Registry" or a "Menu". 
// Here, you are registering all the tools (Services) your application might need later.
// You are telling the application: "Whenever someone asks for 'AppDbContext', 
// please build it using SQL Server and this specific connection string."
var services = new ServiceCollection();

services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// --- WHAT IS BuildServiceProvider? ---
// Once you finish writing your "Menu" (ServiceCollection), you build it into a "Provider".
// The IServiceProvider is the actual engine (The Waiter). 
// It reads the menu and serves you the fully constructed objects whenever you ask for them.
IServiceProvider Sp = services.BuildServiceProvider();


// ==========================================
// CRUD METHODS
// ==========================================

// Notice: We now pass the IServiceProvider instead of DbContextOptions.
// Note for the future: In ASP.NET Core (Web API), you won't even need to pass IServiceProvider. 
// The framework will automatically inject the AppDbContext directly into your class Constructor!

static void GetWallets(IServiceProvider Sp)
{
    // PRO TIP: Use GetRequiredService instead of GetService.
    // GetRequiredService will throw a clear error if the service isn't registered, 
    // whereas GetService might return null and cause confusing NullReferenceExceptions later.
    using (var context = Sp.GetRequiredService<AppDbContext>())
    {
        foreach (var item in context.Wallets)
        {
            Console.WriteLine(item);
        }
    }
}

static void InsetWallet(IServiceProvider Sp)
{
    Console.WriteLine("Adding Wallet : ");
    Console.Write("Please enter the holder name : ");
    string name = Console.ReadLine()!;
    Console.Write("Please enter the Balance of the account : ");
    decimal balance = decimal.Parse(Console.ReadLine()!);

    using (var context = Sp.GetRequiredService<AppDbContext>())
    {
        context.Wallets.Add(new Wallet
        {
            Holder = name,
            Balance = balance
        });

        context.SaveChanges();
    }
}

static void UpdateWallet(IServiceProvider Sp)
{
    Console.Write("Please enter the ID of the account : ");
    int id = Convert.ToInt32(Console.ReadLine()!);

    using (var context = Sp.GetRequiredService<AppDbContext>())
    {
        Console.WriteLine("Updating Wallet : ");

        var UpdatedWallet = context.Wallets.Find(id);

        if (UpdatedWallet == null)
        {
            Console.WriteLine("Wallet not found!");
            return;
        }

        Console.Write("please enter the new Balance : ");
        decimal balance = Decimal.Parse(Console.ReadLine()!);

        UpdatedWallet.Balance = balance;

        context.SaveChanges();
    }
}

static void DeleteWallet(IServiceProvider Sp)
{
    Console.WriteLine("Deleting Wallet : ");
    Console.Write("Please enter the Id of the account : ");
    int Id = Int32.Parse(Console.ReadLine()!);

    using (var context = Sp.GetRequiredService<AppDbContext>())
    {
        var walletToDelete = context.Wallets.Find(Id);

        if (walletToDelete != null)
        {
            context.Wallets.Remove(walletToDelete);
            context.SaveChanges();
            Console.WriteLine("Deleted Successfully.");
        }
        else
        {
            Console.WriteLine("Wallet not found.");
        }
    }
}

static void QueryData(IServiceProvider Sp)
{
    Console.WriteLine();
    using (var context = Sp.GetRequiredService<AppDbContext>())
    {
        var result = context.Wallets.Where(w => w.Balance > 4000m);

        Console.WriteLine("The result after querying the data (balance > 4000) : ");
        foreach (var item in result)
        {
            Console.WriteLine(item);
        }
    }
}

static void Transacion(IServiceProvider Sp)
{
    Console.WriteLine("Transactions in EF CORE : ");
    using (var context = Sp.GetRequiredService<AppDbContext>())
    {
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                Console.Write("Please enter the account to debit from : ");
                int DebitId = Int32.Parse(Console.ReadLine()!);
                Console.Write("Please enter the account to credit to : ");
                int CreditId = Int32.Parse(Console.ReadLine()!);
                Console.Write("Please enter the amount : ");
                decimal Amount = Decimal.Parse(Console.ReadLine()!);

                Wallet DebitWallet = context.Wallets.Single(w => w.Id == DebitId);
                Wallet CreditWallet = context.Wallets.Single(w => w.Id == CreditId);

                DebitWallet.Balance -= Amount;
                CreditWallet.Balance += Amount;

                context.SaveChanges();

                transaction.Commit();
                Console.WriteLine("Transaction Successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction Failed and Rolled Back: {ex.Message}");
            }
        }
    }
}

// ==========================================
// EXECUTION AREA
// ==========================================

Console.WriteLine("Printing All the wallets from the database : ");
GetWallets(Sp);