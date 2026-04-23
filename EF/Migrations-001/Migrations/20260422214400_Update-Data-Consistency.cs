using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Migrations_001.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataConsistency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseName", "Price" },
                values: new object[,]
                {
                    { 1, "Mathmatics", 1000m },
                    { 2, "Physics", 2000m },
                    { 3, "Chemistry", 1500m },
                    { 4, "Biology", 1200m },
                    { 5, "CS-50", 3000m }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Ahmed", "Abdullah" },
                    { 2, "Yasmeen", "Yasmeen" },
                    { 3, "Khalid", "Hassan" },
                    { 4, "Nadia", "Ali" },
                    { 5, "Ahmed", "Abdullah" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Instructors");
        }
    }
}
