
// ==========================================
// EF CORE: BASICS & CRUD OPERATIONS
// ==========================================

// --- WHY EXTERNAL CONFIGURATION? ---
// Here we send the options as parameters (External) instead of hardcoding them inside the DbContext (Internal).
// Benefits:
// 1. Separation of Concerns: The DbContext class stays clean; it only cares about your data (DbSets), not the database connection details.
// 2. Flexibility & Testing: You can easily switch the database (e.g., using an In-Memory DB for Unit Testing) without modifying the DbContext code.
// 3. DI Ready: This is exactly how ASP.NET Core Dependency Injection works under the hood.

// --- GLOBAL VS LOCAL DECLARATIONS ---
// Q: Is it better to declare the configuration object inside each method below?
// A: ABSOLUTELY NOT. You should build the configuration and connection string ONCE at the application startup (Globally).
// If you put it inside each method, the application will read the physical "appsettings.json" file from the hard drive every time the method is called. Disk I/O is very slow and will ruin your app's performance.

// A. Build the Configuration Object
// This links our C# code to the "appsettings.json" file to read settings securely.
using EF002_ExternalConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// B. Get the Connection String
// PRO TIP: Instead of GetSection("constr").Value, use the built-in shortcut!
// This automatically looks for a key inside a "ConnectionStrings" block in your JSON.
string connectionString = configuration.GetConnectionString("DefaultConnection")
                          ?? configuration.GetSection("constr").Value!;

// --- WHAT IS DbContextOptionsBuilder? ---
// It is an EF Core configuration tool. Its job is to collect all the settings your DbContext needs 
// (like which DB provider to use, connection string, logging mechanisms, etc.) and package them 
// into a single "Options" object that can be passed to the DbContext constructor.

// Best Practice: Use the generic version <AppDbContext> for type safety.
DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

// C. Tell EF Core to use SQL Server with this connection string
optionsBuilder.UseSqlServer(connectionString);

// Store the final options in a variable to pass to our methods
var dbOptions = optionsBuilder.Options;

// ==========================================
// CRUD METHODS
// ==========================================

// Best Practice: Pass the options as a parameter to simulate how Dependency Injection works.
static void GetWallets(DbContextOptionsBuilder optionsBuilder)
{
    // Inject the options into the DbContext via its constructor.
    // The 'using' block ensures the connection is closed and memory is freed immediately after the operation.
    using (var context = new AppDbContext(optionsBuilder.Options))
    {
        foreach (var item in context.Wallets)
        {
            Console.WriteLine(item);
        }
    }
}



static void InsetWallet(DbContextOptionsBuilder optionsBuilder)
{
    Console.WriteLine("Adding Wallet : ");
    Console.Write("Please enter the holder name : ");
    string name = Console.ReadLine()!;
    Console.Write("Please enter the Balance of the account : ");
    decimal balance = decimal.Parse(Console.ReadLine()!);

    using (var context = new AppDbContext(optionsBuilder.Options))
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

static void UpdateWallet(DbContextOptionsBuilder optionsBuilder)
{
    Console.Write("Please enter the ID of the account : ");
    int id = Convert.ToInt32(Console.ReadLine()!);
    using (var context = new AppDbContext(optionsBuilder.Options))
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
        var UpdatedWallet = context.Wallets.Find(id);

        Console.Write("please enter the new Balance : ");
        decimal balance = Decimal.Parse(Console.ReadLine()!);

        UpdatedWallet.Balance = balance; // ChangeTracker detects this as "Modified"

        context.SaveChanges(); // Generates and executes the UPDATE SQL query
    }
}

static void DeleteWallet(DbContextOptionsBuilder optionsBuilder)
{
    Console.WriteLine("Deleting Wallet : ");
    Console.Write("Please enter the Id of the account : ");
    int Id = Int32.Parse(Console.ReadLine()!);

    using (var context = new AppDbContext(optionsBuilder.Options))
    {
        // Notice: To delete, you must fetch it first so the ChangeTracker knows about it.
        context.Wallets.Remove(context.Wallets.Single(w => w.Id == Id));

        context.SaveChanges(); // Generates and executes the DELETE SQL query
    }
}

static void QueryData(DbContextOptionsBuilder optionsBuilder)
{
    Console.WriteLine();
    using (var context = new AppDbContext(optionsBuilder.Options))
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

static void Transacion(DbContextOptionsBuilder optionsBuilder)
{
    Console.WriteLine("Transactions in EF CORE : ");
    using (var context = new AppDbContext(optionsBuilder.Options))
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
                int DebitId = Int32.Parse(Console.ReadLine()!);
                Console.Write("Please enter the account to credit to : ");
                int CreditId = Int32.Parse(Console.ReadLine()!);
                Console.Write("Please enter the amount : "); // Fixed WriteLine to Write
                decimal Amount = Decimal.Parse(Console.ReadLine()!);

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
GetWallets(optionsBuilder);

