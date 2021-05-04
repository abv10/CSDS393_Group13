using Microsoft.EntityFrameworkCore.Migrations;

namespace FourYearClassPlanningTool.Migrations.Users
{
    public partial class Modifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Schedules_ScheduleID",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_UserID",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Admins_AdminID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Users_AdminID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ScheduleID",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_UserID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "AdminID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ScheduleID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Courses");

            migrationBuilder.CreateTable(
                name: "CourseSchedule",
                columns: table => new
                {
                    CoursesCourseID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScheduleID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSchedule", x => new { x.CoursesCourseID, x.ScheduleID });
                    table.ForeignKey(
                        name: "FK_CourseSchedule_Courses_CoursesCourseID",
                        column: x => x.CoursesCourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSchedule_Schedules_ScheduleID",
                        column: x => x.ScheduleID,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseUser",
                columns: table => new
                {
                    CompletedCoursesCourseID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersUserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUser", x => new { x.CompletedCoursesCourseID, x.UsersUserID });
                    table.ForeignKey(
                        name: "FK_CourseUser_Courses_CompletedCoursesCourseID",
                        column: x => x.CompletedCoursesCourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseUser_Users_UsersUserID",
                        column: x => x.UsersUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleUser",
                columns: table => new
                {
                    SchedulesScheduleID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersUserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleUser", x => new { x.SchedulesScheduleID, x.UsersUserID });
                    table.ForeignKey(
                        name: "FK_ScheduleUser_Schedules_SchedulesScheduleID",
                        column: x => x.SchedulesScheduleID,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleUser_Users_UsersUserID",
                        column: x => x.UsersUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedule_ScheduleID",
                table: "CourseSchedule",
                column: "ScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_UsersUserID",
                table: "CourseUser",
                column: "UsersUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleUser_UsersUserID",
                table: "ScheduleUser",
                column: "UsersUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseSchedule");

            migrationBuilder.DropTable(
                name: "CourseUser");

            migrationBuilder.DropTable(
                name: "ScheduleUser");

            migrationBuilder.AddColumn<string>(
                name: "AdminID",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScheduleID",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserSchedules",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScheduleID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSchedules", x => x.UserID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AdminID",
                table: "Users",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ScheduleID",
                table: "Courses",
                column: "ScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_UserID",
                table: "Courses",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Schedules_ScheduleID",
                table: "Courses",
                column: "ScheduleID",
                principalTable: "Schedules",
                principalColumn: "ScheduleID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_UserID",
                table: "Courses",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Admins_AdminID",
                table: "Users",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "AdminID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
