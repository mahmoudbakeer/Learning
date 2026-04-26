using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_TPH.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentConfigurationForTPH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "StudentType",
                table: "Students",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentType",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Students",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }
    }
}
