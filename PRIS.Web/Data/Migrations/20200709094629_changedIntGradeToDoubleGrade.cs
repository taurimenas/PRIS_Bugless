using Microsoft.EntityFrameworkCore.Migrations;

namespace PRIS.Web.Data.Migrations
{
    public partial class changedIntGradeToDoubleGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Grade",
                table: "ConversationResult",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "ConversationResult",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
