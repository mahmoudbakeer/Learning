using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_TPC.Migrations
{
    /// <inheritdoc />
    public partial class AddTPCToQuzeClassAndItsDerivedClassed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MultipleChoiceQuizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrcectAnswer = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceQuizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultipleChoiceQuizes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrueOrFalseQuizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TrueOrFalse = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrueOrFalseQuizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrueOrFalseQuizes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceQuizes_CourseId",
                table: "MultipleChoiceQuizes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TrueOrFalseQuizes_CourseId",
                table: "TrueOrFalseQuizes",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultipleChoiceQuizes");

            migrationBuilder.DropTable(
                name: "TrueOrFalseQuizes");
        }
    }
}
