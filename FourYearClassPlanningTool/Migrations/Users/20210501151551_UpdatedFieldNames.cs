using Microsoft.EntityFrameworkCore.Migrations;

namespace FourYearClassPlanningTool.Migrations.Users
{
    public partial class UpdatedFieldNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_Courses_CoursesCourseID",
                table: "CourseSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_Schedules_ScheduleID",
                table: "CourseSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Courses_CompletedCoursesCourseID",
                table: "CourseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Users_UsersUserID",
                table: "CourseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleUser_Schedules_SchedulesScheduleID",
                table: "ScheduleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleUser_Users_UsersUserID",
                table: "ScheduleUser");

            migrationBuilder.DropColumn(
                name: "SemesterOffered",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UsersUserID",
                table: "ScheduleUser",
                newName: "UsersUserId");

            migrationBuilder.RenameColumn(
                name: "SchedulesScheduleID",
                table: "ScheduleUser",
                newName: "SchedulesScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleUser_UsersUserID",
                table: "ScheduleUser",
                newName: "IX_ScheduleUser_UsersUserId");

            migrationBuilder.RenameColumn(
                name: "ScheduleID",
                table: "Schedules",
                newName: "ScheduleId");

            migrationBuilder.RenameColumn(
                name: "UsersUserID",
                table: "CourseUser",
                newName: "UsersUserId");

            migrationBuilder.RenameColumn(
                name: "CompletedCoursesCourseID",
                table: "CourseUser",
                newName: "CompletedCoursesCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseUser_UsersUserID",
                table: "CourseUser",
                newName: "IX_CourseUser_UsersUserId");

            migrationBuilder.RenameColumn(
                name: "ScheduleID",
                table: "CourseSchedule",
                newName: "ScheduleId");

            migrationBuilder.RenameColumn(
                name: "CoursesCourseID",
                table: "CourseSchedule",
                newName: "CoursesCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSchedule_ScheduleID",
                table: "CourseSchedule",
                newName: "IX_CourseSchedule_ScheduleId");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "Courses",
                newName: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_Courses_CoursesCourseId",
                table: "CourseSchedule",
                column: "CoursesCourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_Schedules_ScheduleId",
                table: "CourseSchedule",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Courses_CompletedCoursesCourseId",
                table: "CourseUser",
                column: "CompletedCoursesCourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Users_UsersUserId",
                table: "CourseUser",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleUser_Schedules_SchedulesScheduleId",
                table: "ScheduleUser",
                column: "SchedulesScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleUser_Users_UsersUserId",
                table: "ScheduleUser",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_Courses_CoursesCourseId",
                table: "CourseSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_Schedules_ScheduleId",
                table: "CourseSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Courses_CompletedCoursesCourseId",
                table: "CourseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Users_UsersUserId",
                table: "CourseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleUser_Schedules_SchedulesScheduleId",
                table: "ScheduleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleUser_Users_UsersUserId",
                table: "ScheduleUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "UsersUserId",
                table: "ScheduleUser",
                newName: "UsersUserID");

            migrationBuilder.RenameColumn(
                name: "SchedulesScheduleId",
                table: "ScheduleUser",
                newName: "SchedulesScheduleID");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleUser_UsersUserId",
                table: "ScheduleUser",
                newName: "IX_ScheduleUser_UsersUserID");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "Schedules",
                newName: "ScheduleID");

            migrationBuilder.RenameColumn(
                name: "UsersUserId",
                table: "CourseUser",
                newName: "UsersUserID");

            migrationBuilder.RenameColumn(
                name: "CompletedCoursesCourseId",
                table: "CourseUser",
                newName: "CompletedCoursesCourseID");

            migrationBuilder.RenameIndex(
                name: "IX_CourseUser_UsersUserId",
                table: "CourseUser",
                newName: "IX_CourseUser_UsersUserID");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "CourseSchedule",
                newName: "ScheduleID");

            migrationBuilder.RenameColumn(
                name: "CoursesCourseId",
                table: "CourseSchedule",
                newName: "CoursesCourseID");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSchedule_ScheduleId",
                table: "CourseSchedule",
                newName: "IX_CourseSchedule_ScheduleID");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Courses",
                newName: "CourseID");

            migrationBuilder.AddColumn<string>(
                name: "SemesterOffered",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_Courses_CoursesCourseID",
                table: "CourseSchedule",
                column: "CoursesCourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_Schedules_ScheduleID",
                table: "CourseSchedule",
                column: "ScheduleID",
                principalTable: "Schedules",
                principalColumn: "ScheduleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Courses_CompletedCoursesCourseID",
                table: "CourseUser",
                column: "CompletedCoursesCourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Users_UsersUserID",
                table: "CourseUser",
                column: "UsersUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleUser_Schedules_SchedulesScheduleID",
                table: "ScheduleUser",
                column: "SchedulesScheduleID",
                principalTable: "Schedules",
                principalColumn: "ScheduleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleUser_Users_UsersUserID",
                table: "ScheduleUser",
                column: "UsersUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
