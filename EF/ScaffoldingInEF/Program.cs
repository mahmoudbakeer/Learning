using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
Note: Entity Framework Core Scaffolding (Reverse Engineering) Guide

1. What is Scaffolding?
   Scaffolding is the process of automatically generating C# entity classes and 
   a DbContext from an existing relational database schema.

2. General Steps to Scaffold:
   Step A: Ensure your project builds perfectly (no compiler errors).
   Step B: Install the required provider and tooling packages via PMC:
           Install-Package Microsoft.EntityFrameworkCore.SqlServer
           Install-Package Microsoft.EntityFrameworkCore.Design
           Install-Package Microsoft.EntityFrameworkCore.Tools
   Step C: Run the Scaffold-DbContext command in the Package Manager Console.

3. Command Construction and Customizations:
   Below is the complete command incorporating all your requested switches.
*/

// Scaffold-DbContext "Data Source=.;Initial Catalog=TechTalk;Integrated Security=SSPI;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -ContextDir Data -Context "TechTalkDbContext" -Tables "Speakers", "Events" -DataAnnotations -UseDatabaseNames 

/*
4. Explanation of the Switches:
   -OutputDir Entities      : Places the generated Entity classes into the 'Entities' directory.
   -ContextDir Data         : Places the generated DbContext class into the 'Data' directory.
   -Context "CustomName"    : Assigns a specific name to the DbContext (e.g., TechTalkDbContext).
   -Tables "T1", "T2"       : Instructs EF Core to only scaffold the specified tables.
   -DataAnnotations         : Applies attributes (e.g., [Key], [Column]) directly to the properties 
                              instead of configuring everything via Fluent API in OnModelCreating.
   -UseDatabaseNames        : Preserves the exact table and column names from the database, 
                              preventing EF Core from altering casing or pluralization.
   -Force                   : Overwrites previously generated files when updating the schema.

5. Important Notes:
   - Build Requirement: The command executes a project build behind the scenes. 
     Top-level statements or a static Main method must be valid and error-free.
   - Security: Avoid leaving production connection strings in the generated DbContext file. 
     Move them to appsettings.json or user secrets.
   - Updates: If the database schema changes, you must re-run the command with the -Force 
     switch to update your existing classes.
*/



// -- 2 --
/*
Note: Entity Framework Core Scaffolding using .NET CLI (dotnet ef)


2. General Steps to Scaffold via CLI:
   Step A: Ensure your project builds without errors (run: dotnet build).
   Step B: Install the EF Core CLI tool if you haven't already:
           dotnet tool install --global dotnet-ef
   Step C: Add the required provider and design packages to your project:
           dotnet add package Microsoft.EntityFrameworkCore.SqlServer
           dotnet add package Microsoft.EntityFrameworkCore.Design
   Step D: Run the scaffolding command in your terminal from the project directory.

3. Command Construction and Customizations:
   Below is the complete CLI command incorporating all your requested options.
*/

// dotnet ef dbcontext scaffold "Data Source=.;Initial Catalog=TechTalk;Integrated Security=SSPI;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir Entities --context-dir Data --context TechTalkDbContext --table Speakers --table Events --data-annotations --use-database-names 

/*
4. Explanation of the CLI Switches:
   --output-dir Entities   : Places the generated Entity classes into the 'Entities' directory (Short: -o).
   --context-dir Data      : Places the generated DbContext class into the 'Data' directory.
   --context "Name"        : Assigns a specific name to the DbContext (Short: -c).
   --table "TableName"     : Scaffolds only the specified table. You must repeat this 
                             flag for each table (e.g., --table Users --table Posts) (Short: -t).
   --data-annotations      : Applies attributes (e.g., [Key], [Column]) directly to the properties.
   --use-database-names    : Preserves the exact table and column names from the database, 
                             preventing EF Core from altering casing or pluralization.
   --force                 : Overwrites previously generated files when updating the schema (Short: -f).

5. Important Notes:
   - Directory Execution: Unlike the Package Manager Console, CLI commands must be run in 
     a standard terminal (like PowerShell or CMD) from the folder containing your .csproj file.
   - Tool Versions: Ensure your global 'dotnet-ef' tool version aligns with the EF Core 
     package versions installed in your project (e.g., both should be 9.0.x).
*/
using ScaffoldingInEF.Data;
using ScaffoldingInEF.Entities;
namespace ScaffoldingInEF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            foreach (var item in new TechTalkDbContext().Speakers)
                Console.WriteLine($"{item.FirstName} - {item.LastName}");
        }
    }
}
