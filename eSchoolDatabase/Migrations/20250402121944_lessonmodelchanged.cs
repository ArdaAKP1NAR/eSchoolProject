using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eSchoolDatabase.Migrations
{
    /// <inheritdoc />
    public partial class lessonmodelchanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Classes_ClassId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ClassId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Lessons");

            migrationBuilder.AddColumn<long>(
                name: "LessonId",
                table: "Classes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_LessonId",
                table: "Classes",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Lessons_LessonId",
                table: "Classes",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Lessons_LessonId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_LessonId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Classes");

            migrationBuilder.AddColumn<long>(
                name: "ClassId",
                table: "Lessons",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ClassId",
                table: "Lessons",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Classes_ClassId",
                table: "Lessons",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
