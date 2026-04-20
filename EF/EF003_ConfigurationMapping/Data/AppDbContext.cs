using EF003_ConfigurationMapping.Data.Config;
using EF003_ConfigurationMapping.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace EF003_ConfigurationMapping.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            string connstr = config.GetConnectionString("FakeTwitterV1");
            optionsBuilder.UseSqlServer(connstr);
        }

        // We can configure our database models here using the Fluent API.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Compared to Data Annotations and Convention-based approaches, 
            // Fluent API configuration is much better and essential to know.

            // We can pass the table name directly here. However, doing this 
            // for all entities breaks the Separation of Concerns principle. It turns this 
            // method into a massive, hard-to-maintain method.

            // The better approach is to group configurations into separate files 
            // within a dedicated folder to keep AppDbContext clean.

            // modelBuilder.Entity<Tweet>().ToTable("tblTweets");
            // modelBuilder.Entity<User>().ToTable("tblUsers");
            // modelBuilder.Entity<Comment>().ToTable("tblComments");

            // Fluent API is indeed better than Data Annotations because 
            // it keeps the Entity classes (POCOs) completely free of database-specific code.

            // To map a specific property to a custom database column name, 
            // we use the HasColumnName() method.
            // modelBuilder.Entity<Tweet>().Property(p => p.Id).HasColumnName("TweetId");


            // Applying individual configuration classes manually:
            new UserConfiguration().Configure(modelBuilder.Entity<User>());
            new TweetConfiguration().Configure(modelBuilder.Entity<Tweet>());
            new CommentConfiguration().Configure(modelBuilder.Entity<Comment>());

            // This approach doesn't make a huge difference. If we had 
            // 50 tables, we would still have to write 50 lines here, which is not scalable.

            // EF Core provides a powerful feature called "Group Configuration" 
            // by scanning the Assembly.
            modelBuilder.ApplyConfigurationsFromAssembly(
                // Using typeof(UserConfiguration).Assembly works perfectly. 
                // Alternatively, you can use Assembly.GetExecutingAssembly() if the 
                // configurations are in the same project. Both methods scan the project 
                // for any class implementing IEntityTypeConfiguration and apply it automatically.
                typeof(UserConfiguration).Assembly
            );
        }
    }
}