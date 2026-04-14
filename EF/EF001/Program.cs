using System;
using System.Linq;

// ==========================================
// EF CORE: BASICS & CRUD OPERATIONS
// ==========================================

static void GetWallets()
{
    using (var context = new AppDbContext())
    {
        foreach (var item in context.Wallets)
        {
            Console.WriteLine(item);
        }
    }
}

static void InsetWallet()
{
    Console.WriteLine("Adding Wallet : ");
    Console.Write("Please enter the holder name : ");
    string name = Console.ReadLine();
    Console.Write("Please enter the Balance of the account : ");
    decimal balance = decimal.Parse(Console.ReadLine());

    using (var context = new AppDbContext())
    {
        context.Wallets.Add(new Wallet
        {
            Holder = name,
            Balance = balance
        });

        // ==========================================
        // EXPLAINING SaveChanges()
        // ==========================================
        // SIMPLE WAY:
        // It acts like a "Submit" button. Any Add, Update, or Delete you perform 
        // only happens locally in the application's memory. SaveChanges() is what 
        // actually pushes those changes to the real database.
        //
        // DETAILED WAY:
        // EF Core relies on a "ChangeTracker". When you interact with entities, 
        // the tracker flags their State (Added, Modified, or Deleted).
        // When you execute SaveChanges():
        // 1. It reads the tracker and generates the exact SQL commands (INSERT, UPDATE, DELETE).
        // 2. It wraps these commands inside an implicit Database Transaction (All or Nothing).
        // 3. It sends them to the DB and returns an integer representing the number of affected rows.
        // ==========================================
        context.SaveChanges();
    }
}

static void UpdateWallet()
{
    Console.Write("Please enter the ID of the account : ");
    int id = Convert.ToInt32(Console.ReadLine());
    using (var context = new AppDbContext())
    {
        Console.WriteLine("Updating Wallet : ");

        // ==========================================
        // BEST WAY TO FETCH & UPDATE A RECORD
        // ==========================================
        // 1. Fetching (The Tracking Way):
        // Avoid using .Single() when searching by Primary Key. If the ID doesn't exist, 
        // your app will CRASH (Exception). Instead, use .Find(id) or .FirstOrDefault().
        // .Find() is highly optimized because it checks the local memory first before hitting the DB.
        //
        // Example: var wallet = context.Wallets.Find(id); 
        //          if(wallet == null) return; // Handle gracefully
        //
        // 2. The Ultimate High-Performance Way (.NET 7+ Feature):
        // If you only want to update the balance without loading the whole object into memory, 
        // use ExecuteUpdate(). It fires an UPDATE SQL statement directly!
        // Example: 
        // context.Wallets.Where(w => w.Id == id)
        //                .ExecuteUpdate(s => s.SetProperty(w => w.Balance, balance));
        // ==========================================

        // Falling back to your original method for the example:
        var UpdatedWallet = context.Wallets.Single(w => w.Id == id);

        Console.Write("please enter the new Balance : ");
        decimal balance = Decimal.Parse(Console.ReadLine());

        UpdatedWallet.Balance = balance; // ChangeTracker detects this as "Modified"

        context.SaveChanges(); // Generates and executes the UPDATE SQL query
    }
}

static void DeleteWallet()
{
    Console.WriteLine("Deleting Wallet : ");
    Console.Write("Please enter the Id of the account : ");
    int Id = Int32.Parse(Console.ReadLine());

    using (var context = new AppDbContext())
    {
        // Notice: To delete, you must fetch it first so the ChangeTracker knows about it.
        context.Wallets.Remove(context.Wallets.Single(w => w.Id == Id));

        context.SaveChanges(); // Generates and executes the DELETE SQL query
    }
}

static void QueryData()
{
    Console.WriteLine();
    using (var context = new AppDbContext())
    {
        // LINQ queries are converted directly into SQL "SELECT ... WHERE" statements
        var result = context.Wallets.Where(w => w.Balance > 4000m);

        Console.WriteLine("The result after querying the data (balance > 4000) : ");
        foreach (var item in result)
        {
            Console.WriteLine(item);
        }
    }
}

static void Transacion()
{
    Console.WriteLine("Transactions in EF CORE : ");
    using (var context = new AppDbContext())
    {
        // ==========================================
        // EXPLAINING context.Database (DatabaseFacade)
        // ==========================================
        // The 'Database' property returns an object of type 'DatabaseFacade'.
        // While 'context.Wallets' is used to manipulate your DATA (rows), 
        // 'context.Database' is used to manipulate the DATABASE ENGINE itself.
        //
        // What does it do?
        // It acts as the bridge to your DB provider (like SQL Server or PostgreSQL).
        // It provides infrastructure-level methods such as:
        // 1. Managing manual Transactions (.BeginTransaction).
        // 2. Executing raw SQL strings (.ExecuteSqlRaw).
        // 3. Creating or dropping the database schema (.EnsureCreated, .Migrate).
        // ==========================================

        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                Console.Write("Please enter the account to debit from : ");
                int DebitId = Int32.Parse(Console.ReadLine());
                Console.Write("Please enter the account to credit to : ");
                int CreditId = Int32.Parse(Console.ReadLine());
                Console.Write("Please enter the amount : "); // Fixed WriteLine to Write
                decimal Amount = Decimal.Parse(Console.ReadLine());

                Wallet DebitWallet = context.Wallets.Single(w => w.Id == DebitId);
                Wallet CreditWallet = context.Wallets.Single(w => w.Id == CreditId);

                // The logical updates
                DebitWallet.Balance -= Amount;
                CreditWallet.Balance += Amount;

                context.SaveChanges(); // Pushes changes, but they are NOT permanent yet!

                transaction.Commit(); // Locks it in. Without this, changes are rolled back.
                Console.WriteLine("Transaction Successful!");
            }
            catch (Exception ex)
            {
                // If anything fails (like Single finding no record), 
                // the transaction will automatically Rollback when the 'using' block ends.
                Console.WriteLine($"Transaction Failed and Rolled Back: {ex.Message}");
            }
        }
    }
}

// ==========================================
// EXECUTION AREA
// ==========================================
// Console.WriteLine("Printing All the wallets from the database : ");
// GetWallets();
// Console.WriteLine("Adding an wallet to database : ");
// InsetWallet();
// Console.WriteLine("Printing All the wallets from the database : ");
// GetWallets();
// UpdateWallet();
// Console.WriteLine("Printing All the wallets from the database : ");
// GetWallets();
// DeleteWallet();

Console.WriteLine("Printing All the wallets from the database : ");
GetWallets();
QueryData();
Transacion();
