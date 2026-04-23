using EntityTypesAndMapping.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EntityTypesAndMapping.Data
{
    public class AppDbContext : DbContext
    {
        // Clarification: Initializing DbSets using '=> Set<T>()' is a best practice 
        // to avoid uninitialized non-nullable property warnings in modern C#.

        // public DbSet<Order> Orders { get; set; }
        public DbSet<Order> Orders => Set<Order>();

        // public DbSet<Product> Products { get; set; }
        public DbSet<Product> Products => Set<Product>();

        // public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        // public DbSet<OrderWithDetailsView> OrderWithDetailsView { get; set; }
        public DbSet<OrderWithDetailsView> OrderWithDetailsView => Set<OrderWithDetailsView>();

        // public DbSet<OrderBill> OrderGivenBill { get; set; }
        public DbSet<OrderBill> OrderGivenBill => Set<OrderBill>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Clarification: It is a standard practice to call the base method at the 
            // beginning of OnModelCreating, particularly if inheriting from complex 
            // contexts like IdentityDbContext where foundational configurations occur.
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                // Maps the entity to the "OrderDetails" table within the "Sales" schema.
                entity.ToTable("OrderDetails", schema: "Sales");

                // Configures a composite primary key using both OrderId and ProductId.
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                // Configures the one-to-many relationship from Order to OrderDetail.
                // An Order can have many OrderDetails. The foreign key is OrderId.
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(e => e.OrderId);

                // Configures the relationship from Product to OrderDetail.
                // A Product can be associated with many OrderDetails, but the Product 
                // entity does not require a navigation property back to OrderDetail.
                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                // Maps the Product entity to the "Products" table in the "Inventory" schema.
                entity.ToTable("Products", schema: "Inventory");

                // Explicitly configures the primary key. EF Core automatically infers "Id", 
                // but writing it explicitly improves readability and intent.
                entity.HasKey(p => p.Id);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                // Maps the Order entity to the "Orders" table in the "Sales" schema.
                entity.ToTable("Orders", schema: "Sales");

                // Explicitly configures the primary key.
                entity.HasKey(o => o.Id);
            });

            // Instructs EF Core to exclude the SnapShot class entirely from the model.
            // It will not be mapped to any database table or view.
            modelBuilder.Ignore<SnapShot>();

            modelBuilder.Entity<OrderWithDetailsView>(entity =>
            {
                // Maps this entity to a database view instead of a table.
                entity.ToView("OrderWithDetailsView");

                // Indicates that this entity does not have a primary key.
                // Entities without keys are handled as strictly read-only in EF Core.
                entity.HasNoKey();
            });

            modelBuilder.Entity<OrderBill>(entity =>
            {
                // Indicates no primary key, enforcing read-only behavior.
                entity.HasNoKey();

                // Maps the entity to a Table-Valued Function (TVF) named "GetOrderBill".
                entity.ToFunction("GetOrderBill");
            });

            // The old base call placement. It has been moved to the top of the method.
            // base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Note: Reading configuration directly inside OnConfiguring is completely valid 
            // for Console applications. However, for ASP.NET Core applications, Dependency 
            // Injection (DI) is the standard best practice, passing DbContextOptions via constructor.

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connStr = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connStr);

            base.OnConfiguring(optionsBuilder);
        }
    }
}