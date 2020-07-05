using Microsoft.EntityFrameworkCore.Migrations;

namespace PRIS.Web.Data.Migrations
{
    public partial class ChangedTasksStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Task1_1",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Task1_2",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Task1_3",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Task2_1",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Task2_2",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Task2_3",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Task3_1",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Task3_2",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Task3_3",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Task3_4",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Task1_1",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "Task1_2",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "Task1_3",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "Task2_1",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "Task2_2",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "Task2_3",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "Task3_1",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "Task3_2",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "Task3_3",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "Task3_4",
                table: "Exam");

            migrationBuilder.AddColumn<string>(
                name: "Tasks",
                table: "Result",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tasks",
                table: "Exam",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tasks",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "Tasks",
                table: "Exam");

            migrationBuilder.AddColumn<double>(
                name: "Task1_1",
                table: "Result",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task1_2",
                table: "Result",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task1_3",
                table: "Result",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task2_1",
                table: "Result",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task2_2",
                table: "Result",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task2_3",
                table: "Result",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task3_1",
                table: "Result",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task3_2",
                table: "Result",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task3_3",
                table: "Result",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task3_4",
                table: "Result",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task1_1",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task1_2",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task1_3",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task2_1",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task2_2",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task2_3",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task3_1",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task3_2",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task3_3",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Task3_4",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
