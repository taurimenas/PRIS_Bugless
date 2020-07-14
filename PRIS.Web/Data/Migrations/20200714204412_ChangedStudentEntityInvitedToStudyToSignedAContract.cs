using Microsoft.EntityFrameworkCore.Migrations;

namespace PRIS.Web.Data.Migrations
{
    public partial class ChangedStudentEntityInvitedToStudyToSignedAContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitedToStudy",
                table: "Student");

            migrationBuilder.AddColumn<bool>(
                name: "SignedAContract",
                table: "Student",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignedAContract",
                table: "Student");

            migrationBuilder.AddColumn<bool>(
                name: "InvitedToStudy",
                table: "Student",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
