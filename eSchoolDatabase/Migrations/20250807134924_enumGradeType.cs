using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eSchoolDatabase.Migrations
{
    /// <inheritdoc />
    public partial class enumGradeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeType",
                table: "Grades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeType",
                table: "Grades");
        }
    }
}
