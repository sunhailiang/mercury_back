using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MercurySurvey.Migrations
{
    public partial class AddQuestionGuidInQuestionQueueTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Question",
                table: "Answer",
                newName: "QuestionVersion");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionGuid",
                table: "Answer",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionGuid",
                table: "Answer");

            migrationBuilder.RenameColumn(
                name: "QuestionVersion",
                table: "Answer",
                newName: "Question");
        }
    }
}
