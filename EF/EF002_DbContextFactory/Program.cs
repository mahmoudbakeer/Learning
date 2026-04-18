using EF002_DbContextFactory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

// ==========================================
// EF CORE: DEPENDENCY INJECTION & DbContextFactory
// ==========================================

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string connectionString = configuration.GetConnectionString("DefaultConnection")
                          ?? configuration.GetSection("constr").Value!;

var services = new ServiceCollection();

// ==========================================
// --- WHAT IS DbContextFactory? ---
// It is a "Factory" (a creator) that generates new, independent instances of your AppDbContext on demand.
// Instead of the DI container giving you the DbContext directly, it gives you a machine (Factory) that MAKES the DbContext.

// --- WHY DO WE USE IT? ---
// 1. Thread Safety: DbContext is NOT thread-safe. If two threads (or operations) try to use the same DbContext at the same time, the app will crash. The factory allows each operation to generate its own isolated DbContext.
// 2. Exact Lifecycle Control: You have full control over when the connection to the database opens (factory.CreateDbContext()) and when it closes (at the end of the 'using' block).

// --- WHEN DO WE USE IT? ---
// 1. Blazor Server Apps: Because users have a long-lived connection (SignalR) rather than short HTTP requests.
// 2. Background/Worker Services: Because there is no "HTTP Request Scope" to automatically clean up the DbContext.
// 3. Desktop & Console Apps: Like this app! It's the safest way to ensure you don't keep a database connection open longer than necessary.
// ==========================================
services.AddDbContextFactory<AppDbContext>(options => options.UseSqlServer(connectionString));

// Build the service provider (The Waiter)
IServiceProvider Sp = services.BuildServiceProvider();

// Here we ask the DI container for the FACTORY, not the DbContext itself.
IDbContextFactory<AppDbContext> contextFactory = Sp.GetRequiredService<IDbContextFactory<AppDbContext>>();

// ==========================================
// CRUD METHODS
// ==========================================

static void GetWallets(IDbContextFactory<AppDbContext> contextFact)
{
    // We use the factory to create a short-lived DbContext just for this specific task.
    // The 'using' block guarantees the DbContext is disposed of immediately after the loop.
    using (var context = contextFact.CreateDbContext())
    {
        foreach (var item in context.Wallets)
        {
            Console.WriteLine(item);
        }
    }
}

static void InsetWallet(IDbContextFactory<AppDbContext> contextFact)
{
    Console.WriteLine("Adding Wallet : ");
    Console.Write("Please enter the holder name : ");
    string name = Console.ReadLine()!;
    Console.Write("Please enter the Balance of the account : ");
    decimal balance = decimal.Parse(Console.ReadLine()!);

    using (var context = contextFact.CreateDbContext())
    {
        context.Wallets.Add(new Wallet
        {
            Holder = name,
            Balance = balance
        });

        context.SaveChanges();
    }
}

static void UpdateWallet(IDbContextFactory<AppDbContext> contextFact)
{
    Console.Write("Please enter the ID of the account : ");
    int id = Convert.ToInt32(Console.ReadLine()!);

    using (var context = contextFact.CreateDbContext())
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

static void DeleteWallet(IDbContextFactory<AppDbContext> contextFact)
{
    Console.WriteLine("Deleting Wallet : ");
    Console.Write("Please enter the Id of the account : ");
    int Id = Int32.Parse(Console.ReadLine()!);

    using (var context = contextFact.CreateDbContext())
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

static void QueryData(IDbContextFactory<AppDbContext> contextFact)
{
    Console.WriteLine();
    using (var context = contextFact.CreateDbContext())
    {
        var result = context.Wallets.Where(w => w.Balance > 4000m);

        Console.WriteLine("The result after querying the data (balance > 4000) : ");
        foreach (var item in result)
        {
            Console.WriteLine(item);
        }
    }
}

static void Transacion(IDbContextFactory<AppDbContext> contextFact)
{
    Console.WriteLine("Transactions in EF CORE : ");
    using (var context = contextFact.CreateDbContext())
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
GetWallets(contextFactory);