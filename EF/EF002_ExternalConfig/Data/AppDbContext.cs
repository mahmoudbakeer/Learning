using EF002_ExternalConfig;
using Microsoft.EntityFrameworkCore;

// ==========================================
// EF CORE: EXTERNAL CONFIGURATION (DEPENDENCY INJECTION)
// The Industry Standard approach for modern ASP.NET Core Web APIs.
// ==========================================
public class AppDbContext : DbContext
{
    // 1. DbSet: Represents a Table in your database. 
    // You will use this property to query and save instances of 'Wallet'.
    // The 'null!' is the null-forgiving operator. It literally means: 
    // "Shut up compiler and trust me, EF Core will initialize this behind the scenes!"
    public DbSet<Wallet> Wallets { get; set; } = null!;


    // ==========================================
    // 2. THE CONSTRUCTOR (Dependency Injection Way)
    // ==========================================
    // WHY DID WE REMOVE 'OnConfiguring' AND ADD THIS CONSTRUCTOR?
    // 
    // 1. Separation of Concerns (SoC):
    //    This class no longer cares WHERE the database is, or WHICH database engine 
    //    (SQL Server, PostgreSQL, SQLite) is being used. It simply says: 
    //    "Pass me the options from the outside, and I will pass them to the base class."
    //
    // 2. ASP.NET Core Integration:
    //    In modern Web APIs, we configure the database connection centrally in the 
    //    'Program.cs' file. The ASP.NET Core DI Container will automatically build the 
    //    'DbContextOptions' object and "inject" it into this constructor when needed.
    //
    // 3. Unit Testing Superpower:
    //    Because the configuration comes from the outside, we can easily pass an 
    //    "In-Memory Database" option when writing automated Unit Tests, without having 
    //    to change a single line of code in this class!
    // ==========================================
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }
}