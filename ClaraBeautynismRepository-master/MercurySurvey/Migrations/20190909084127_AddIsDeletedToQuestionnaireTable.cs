using Microsoft.EntityFrameworkCore.Migrations;

namespace MercurySurvey.Migrations
{
    public partial class AddIsDeletedToQuestionnaireTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Questionnaire",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Questionnaire");
        }
    }
}
