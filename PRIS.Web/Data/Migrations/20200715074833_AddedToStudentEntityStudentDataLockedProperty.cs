using Microsoft.EntityFrameworkCore.Migrations;

namespace PRIS.Web.Data.Migrations
{
    public partial class AddedToStudentEntityStudentDataLockedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "StudentDataLocked",
                table: "Student",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentDataLocked",
                table: "Student");
        }
    }
}
