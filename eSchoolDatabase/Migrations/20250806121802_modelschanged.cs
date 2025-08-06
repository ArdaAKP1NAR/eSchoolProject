using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eSchoolDatabase.Migrations
{
    /// <inheritdoc />
    public partial class modelschanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Lessons_LessonId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Lessons_LessonId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_LessonId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Classes_LessonId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Classes");

            migrationBuilder.AlterColumn<long>(
                name: "StudentId",
                table: "Grades",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ClassLesson",
                columns: table => new
                {
                    ClassListId = table.Column<long>(type: "bigint", nullable: false),
                    LessonsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLesson", x => new { x.ClassListId, x.LessonsId });
                    table.ForeignKey(
                        name: "FK_ClassLesson_Classes_ClassListId",
                        column: x => x.ClassListId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassLesson_Lessons_LessonsId",
                        column: x => x.LessonsId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassLesson_LessonsId",
                table: "ClassLesson",
                column: "LessonsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades");

            migrationBuilder.DropTable(
                name: "ClassLesson");

            migrationBuilder.AddColumn<long>(
                name: "LessonId",
                table: "Students",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "StudentId",
                table: "Grades",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "LessonId",
                table: "Classes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_LessonId",
                table: "Students",
                column: "LessonId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Lessons_LessonId",
                table: "Students",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }
    }
}
