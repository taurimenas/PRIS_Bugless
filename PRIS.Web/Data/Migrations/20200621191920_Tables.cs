using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PRIS.Web.Data.Migrations
{
    public partial class Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConversationResult",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Grade = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    StartYear = table.Column<DateTime>(nullable: false),
                    EndYear = table.Column<DateTime>(nullable: false),
                    ProgramId = table.Column<int>(nullable: false),
                    CityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Task1_1 = table.Column<double>(nullable: false),
                    Task1_2 = table.Column<double>(nullable: false),
                    Task1_3 = table.Column<double>(nullable: false),
                    Task2_1 = table.Column<double>(nullable: false),
                    Task2_2 = table.Column<double>(nullable: false),
                    Task2_3 = table.Column<double>(nullable: false),
                    Task3_1 = table.Column<double>(nullable: false),
                    Task3_2 = table.Column<double>(nullable: false),
                    Task3_3 = table.Column<double>(nullable: false),
                    Task3_4 = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Task1_1 = table.Column<double>(nullable: false),
                    Task1_2 = table.Column<double>(nullable: false),
                    Task1_3 = table.Column<double>(nullable: false),
                    Task2_1 = table.Column<double>(nullable: false),
                    Task2_2 = table.Column<double>(nullable: false),
                    Task2_3 = table.Column<double>(nullable: false),
                    Task3_1 = table.Column<double>(nullable: false),
                    Task3_2 = table.Column<double>(nullable: false),
                    Task3_3 = table.Column<double>(nullable: false),
                    Task3_4 = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CourseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Program",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CourseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Program_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    StudentsCourseId = table.Column<int>(nullable: false),
                    ResultId = table.Column<int>(nullable: true),
                    ConversationResultId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_ConversationResult_ConversationResultId",
                        column: x => x.ConversationResultId,
                        principalTable: "ConversationResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_Result_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Result",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestResult",
                columns: table => new
                {
                    ExamId = table.Column<int>(nullable: false),
                    ResultId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResult", x => new { x.ExamId, x.ResultId });
                    table.ForeignKey(
                        name: "FK_TestResult_Test_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResult_Result_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Result",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentsCourse",
                columns: table => new
                {
                    StudentId = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsCourse", x => new { x.StudentId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_StudentsCourse_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsCourse_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_CourseId",
                table: "City",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Program_CourseId",
                table: "Program",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_ConversationResultId",
                table: "Student",
                column: "ConversationResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_ResultId",
                table: "Student",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsCourse_CourseId",
                table: "StudentsCourse",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_ResultId",
                table: "TestResult",
                column: "ResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Program");

            migrationBuilder.DropTable(
                name: "StudentsCourse");

            migrationBuilder.DropTable(
                name: "TestResult");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "ConversationResult");

            migrationBuilder.DropTable(
                name: "Result");
        }
    }
}
