using Microsoft.EntityFrameworkCore.Migrations;

namespace MercurySurvey.Migrations
{
    public partial class AddDynamicContentToQUestionQueueTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DynamicContent",
                table: "QuestionQueue",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DynamicContent",
                table: "QuestionQueue");
        }
    }
}
