using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations_001.Migrations
{
    /// <inheritdoc />
    public partial class AddValueEnumInsteadOfString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Schedules",
                type: "VARCHAR(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
