using Microsoft.EntityFrameworkCore.Migrations;

namespace FourYearClassPlanningTool.Migrations
{
    public partial class prereqadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prequisites",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prequisites",
                table: "Courses");
        }
    }
}
