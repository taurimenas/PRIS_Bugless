using Microsoft.EntityFrameworkCore.Migrations;

namespace PRIS.Web.Data.Migrations
{
    public partial class ChamgedResultStudentRelashionshipToOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Result_ResultId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_ResultId",
                table: "Student");

            migrationBuilder.AlterColumn<int>(
                name: "StudentsCourseId",
                table: "Student",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StudentForeignKey",
                table: "Result",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Result_StudentForeignKey",
                table: "Result",
                column: "StudentForeignKey",
                unique: true,
                filter: "[StudentForeignKey] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Result_Student_StudentForeignKey",
                table: "Result",
                column: "StudentForeignKey",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Result_Student_StudentForeignKey",
                table: "Result");

            migrationBuilder.DropIndex(
                name: "IX_Result_StudentForeignKey",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "StudentForeignKey",
                table: "Result");

            migrationBuilder.AlterColumn<int>(
                name: "StudentsCourseId",
                table: "Student",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_ResultId",
                table: "Student",
                column: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Result_ResultId",
                table: "Student",
                column: "ResultId",
                principalTable: "Result",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
