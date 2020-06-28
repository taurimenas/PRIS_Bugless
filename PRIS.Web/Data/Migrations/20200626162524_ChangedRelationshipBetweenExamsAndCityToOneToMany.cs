using Microsoft.EntityFrameworkCore.Migrations;

namespace PRIS.Web.Data.Migrations
{
    public partial class ChangedRelationshipBetweenExamsAndCityToOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_Exam_ExamId",
                table: "City");

            migrationBuilder.DropIndex(
                name: "IX_City_ExamId",
                table: "City");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_CityId",
                table: "Exam",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_City_CityId",
                table: "Exam",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_City_CityId",
                table: "Exam");

            migrationBuilder.DropIndex(
                name: "IX_Exam_CityId",
                table: "Exam");

            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "City",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_City_ExamId",
                table: "City",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_City_Exam_ExamId",
                table: "City",
                column: "ExamId",
                principalTable: "Exam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
