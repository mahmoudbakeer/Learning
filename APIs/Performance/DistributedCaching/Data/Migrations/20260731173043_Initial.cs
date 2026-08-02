using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CachingBasics.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("1a2b3c4d-5e6f-7a8b-9c0d-890abcdef123"), "Water", 0.99m },
                    { new Guid("2779ee47-10b0-4bd7-8342-404006aa1392"), "Soda", 1.99m },
                    { new Guid("27a65726-a07f-484c-9b0c-334611ec1c18"), "Milk", 3.49m },
                    { new Guid("2b3c4d5e-6f7a-8b9c-0d1e-90abcdef1234"), "Fruit Juice", 3.99m },
                    { new Guid("2f8b4f29-4d8f-49c1-86f2-234567890abc"), "Butter", 3.99m },
                    { new Guid("3c4d5e6f-7a8b-9c0d-1e2f-abcdef123456"), "Yogurt", 2.99m },
                    { new Guid("3c9f3f00-2b1e-4a3b-a7f9-67890abcdef0"), "Coffee", 7.99m },
                    { new Guid("4d5e6f7a-8b9c-0d1e-2f3a-bcdef1234567"), "Cereal", 4.49m },
                    { new Guid("5e6f7a8b-9c0d-1e2f-3a4b-cdef12345678"), "Rice", 6.99m },
                    { new Guid("5e76a48d-0e75-4e23-9bcd-34567890abcd"), "Eggs", 2.99m },
                    { new Guid("69a0b1fe-3c20-4a4a-bc57-13a8078d8e00"), "Juice", 4.75m },
                    { new Guid("6f7a8b9c-0d1e-2f3a-4b5c-def123456789"), "Pasta", 3.49m },
                    { new Guid("7a8b9c0d-1e2f-3a4b-5c6d-ef1234567890"), "Apple", 0.79m },
                    { new Guid("7d2f3b91-3f2d-4f0a-92c1-4567890abcde"), "Cheese", 5.49m },
                    { new Guid("8b9c0d1e-2f3a-4b5c-6d7e-1234567890ab"), "Banana", 0.59m },
                    { new Guid("8fa9f2a4-1b8a-4e66-ae9b-1234567890ab"), "Bread", 2.49m },
                    { new Guid("9a4c7e3f-5b2d-4e9c-bcde-567890abcdef"), "Chocolate", 1.99m },
                    { new Guid("9c0d1e2f-3a4b-5c6d-7e8f-234567890abc"), "Orange", 0.99m },
                    { new Guid("abcdef12-3456-7890-abcd-ef1234567890"), "Grapes", 2.99m },
                    { new Guid("d5e8f1a2-3c4d-4b5e-89ab-7890abcdef12"), "Tea", 4.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
