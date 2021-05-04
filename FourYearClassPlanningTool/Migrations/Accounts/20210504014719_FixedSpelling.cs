using Microsoft.EntityFrameworkCore.Migrations;

namespace FourYearClassPlanningTool.Migrations.Accounts
{
    public partial class FixedSpelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_Course_CoursesCourseId",
                table: "CourseSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Course_CompletedCoursesCourseId",
                table: "CourseUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_Courses_CoursesCourseId",
                table: "CourseSchedule",
                column: "CoursesCourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Courses_CompletedCoursesCourseId",
                table: "CourseUser",
                column: "CompletedCoursesCourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_Courses_CoursesCourseId",
                table: "CourseSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Courses_CompletedCoursesCourseId",
                table: "CourseUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_Course_CoursesCourseId",
                table: "CourseSchedule",
                column: "CoursesCourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Course_CompletedCoursesCourseId",
                table: "CourseUser",
                column: "CompletedCoursesCourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
