using Microsoft.EntityFrameworkCore.Migrations;

namespace FourYearClassPlanningTool.Migrations
{
    public partial class IntiialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseGroups",
                columns: table => new
                {
                    CourseGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoursesRequired = table.Column<int>(type: "int", nullable: false),
                    CreditsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroups", x => x.CourseGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemestersOffered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Credits = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Degrees",
                columns: table => new
                {
                    DegreeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Concentration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxOverlap = table.Column<int>(type: "int", nullable: false),
                    IsMajor = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degrees", x => x.DegreeId);
                });

            migrationBuilder.CreateTable(
                name: "CourseCourseGroup",
                columns: table => new
                {
                    CourseGroupsCourseGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CoursesCourseId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCourseGroup", x => new { x.CourseGroupsCourseGroupId, x.CoursesCourseId });
                    table.ForeignKey(
                        name: "FK_CourseCourseGroup_CourseGroups_CourseGroupsCourseGroupId",
                        column: x => x.CourseGroupsCourseGroupId,
                        principalTable: "CourseGroups",
                        principalColumn: "CourseGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseCourseGroup_Courses_CoursesCourseId",
                        column: x => x.CoursesCourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseDegree",
                columns: table => new
                {
                    CoursesCourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DegreesDegreeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDegree", x => new { x.CoursesCourseId, x.DegreesDegreeId });
                    table.ForeignKey(
                        name: "FK_CourseDegree_Courses_CoursesCourseId",
                        column: x => x.CoursesCourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDegree_Degrees_DegreesDegreeId",
                        column: x => x.DegreesDegreeId,
                        principalTable: "Degrees",
                        principalColumn: "DegreeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseGroupDegree",
                columns: table => new
                {
                    CourseGroupsCourseGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DegreesDegreeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroupDegree", x => new { x.CourseGroupsCourseGroupId, x.DegreesDegreeId });
                    table.ForeignKey(
                        name: "FK_CourseGroupDegree_CourseGroups_CourseGroupsCourseGroupId",
                        column: x => x.CourseGroupsCourseGroupId,
                        principalTable: "CourseGroups",
                        principalColumn: "CourseGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseGroupDegree_Degrees_DegreesDegreeId",
                        column: x => x.DegreesDegreeId,
                        principalTable: "Degrees",
                        principalColumn: "DegreeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseCourseGroup_CoursesCourseId",
                table: "CourseCourseGroup",
                column: "CoursesCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDegree_DegreesDegreeId",
                table: "CourseDegree",
                column: "DegreesDegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroupDegree_DegreesDegreeId",
                table: "CourseGroupDegree",
                column: "DegreesDegreeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseCourseGroup");

            migrationBuilder.DropTable(
                name: "CourseDegree");

            migrationBuilder.DropTable(
                name: "CourseGroupDegree");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "CourseGroups");

            migrationBuilder.DropTable(
                name: "Degrees");
        }
    }
}
