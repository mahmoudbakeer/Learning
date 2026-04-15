using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
// using System.Configuration; //  Removed: This is legacy .NET Framework. We don't need it in modern .NET.

// ==========================================
// EF CORE: THE DATABASE CONTEXT (AppDbContext)
// This class is the bridge between your C# code and your SQL Database.
// ==========================================
public class AppDbContext : DbContext
{
    // 1. DbSet: Represents a Table in your database. 
    // You will use this property to query and save instances of 'Wallet'.
    // The 'null!' is the null-forgiving operator. It literally means: 
    // "Shut up compiler and trust me, EF Core will initialize this behind the scenes!"
    public DbSet<Wallet> Wallets { get; set; } = null!;


    // 2. OnConfiguring: The setup method.
    // This is where you tell EF Core WHICH database engine to use (SQL Server, SQLite, etc.) 
    // and WHERE to find it (The Connection String).
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // A. Build the Configuration Object
        // This links our C# code to the "appsettings.json" file to read settings securely.
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // B. Get the Connection String
        //  PRO TIP: Instead of GetSection("constr").Value, use the built-in shortcut!
        // This automatically looks for a key inside a "ConnectionStrings" block in your JSON.
        string connectionString = configuration.GetConnectionString("DefaultConnection")
                                  ?? configuration.GetSection("constr").Value;

        // C. Tell EF Core to use SQL Server with this connection string
        optionsBuilder.UseSqlServer(connectionString);
    }
}

d


