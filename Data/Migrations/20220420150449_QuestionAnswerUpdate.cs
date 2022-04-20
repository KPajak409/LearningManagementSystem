using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Data.Migrations
{
    public partial class QuestionAnswerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnswerId",
                table: "Questions",
                newName: "QuestionType");

            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "QuestionType",
                table: "Questions",
                newName: "AnswerId");
        }
    }
}
