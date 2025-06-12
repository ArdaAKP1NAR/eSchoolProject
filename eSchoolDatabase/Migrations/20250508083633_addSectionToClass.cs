using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eSchoolDatabase.Migrations
{
    /// <inheritdoc />
    public partial class addSectionToClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Classes",
                newName: "Section");

            migrationBuilder.AddColumn<string>(
                name: "ClassLevel",
                table: "Classes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassLevel",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "Section",
                table: "Classes",
                newName: "ClassName");
        }
    }
}
