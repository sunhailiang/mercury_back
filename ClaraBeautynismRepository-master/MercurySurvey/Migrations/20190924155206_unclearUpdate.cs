using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MercurySurvey.Migrations
{
    public partial class unclearUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuestionnaireGuid",
                table: "QuestionQueue",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "QuestionnairePK",
                table: "Questionnaire",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnable",
                table: "Question",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QuestionPK",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UploadFile",
                columns: table => new
                {
                    FileGuid = table.Column<Guid>(nullable: false),
                    UserGuid = table.Column<Guid>(nullable: false),
                    Files = table.Column<byte[]>(nullable: true),
                    Ctime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFile", x => x.FileGuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadFile");

            migrationBuilder.DropColumn(
                name: "QuestionnaireGuid",
                table: "QuestionQueue");

            migrationBuilder.DropColumn(
                name: "QuestionnairePK",
                table: "Questionnaire");

            migrationBuilder.DropColumn(
                name: "IsEnable",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "QuestionPK",
                table: "Question");
        }
    }
}
