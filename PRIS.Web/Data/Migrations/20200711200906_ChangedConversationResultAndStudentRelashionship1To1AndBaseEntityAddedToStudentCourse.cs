using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PRIS.Web.Data.Migrations
{
    public partial class ChangedConversationResultAndStudentRelashionship1To1AndBaseEntityAddedToStudentCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_ConversationResult_ConversationResultId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_ConversationResultId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "ConversationResultId",
                table: "Student");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "StudentsCourse",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StudentsCourse",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentForeignKey",
                table: "ConversationResult",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConversationResult_StudentForeignKey",
                table: "ConversationResult",
                column: "StudentForeignKey",
                unique: true,
                filter: "[StudentForeignKey] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationResult_Student_StudentForeignKey",
                table: "ConversationResult",
                column: "StudentForeignKey",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationResult_Student_StudentForeignKey",
                table: "ConversationResult");

            migrationBuilder.DropIndex(
                name: "IX_ConversationResult_StudentForeignKey",
                table: "ConversationResult");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "StudentsCourse");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentsCourse");

            migrationBuilder.DropColumn(
                name: "StudentForeignKey",
                table: "ConversationResult");

            migrationBuilder.AddColumn<int>(
                name: "ConversationResultId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_ConversationResultId",
                table: "Student",
                column: "ConversationResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_ConversationResult_ConversationResultId",
                table: "Student",
                column: "ConversationResultId",
                principalTable: "ConversationResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
