using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MercurySurvey.Migrations
{
    public partial class InitalCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    AnswerGuid = table.Column<Guid>(nullable: false),
                    Userid = table.Column<Guid>(nullable: false),
                    RecordId = table.Column<int>(nullable: false),
                    Ctime = table.Column<DateTime>(nullable: false),
                    Mtime = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Question = table.Column<Guid>(nullable: false),
                    QuestionnaireGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.AnswerGuid);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    QuestionGuid = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Calculator = table.Column<string>(nullable: true),
                    Ctime = table.Column<DateTime>(nullable: false),
                    Version = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.QuestionGuid);
                });

            migrationBuilder.CreateTable(
                name: "Questionnaire",
                columns: table => new
                {
                    QuestionnaireGuid = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Question = table.Column<string>(nullable: true),
                    Calculator = table.Column<string>(nullable: true),
                    Ctime = table.Column<DateTime>(nullable: false),
                    Version = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionnaire", x => x.QuestionnaireGuid);
                });

            migrationBuilder.CreateTable(
                name: "QuestionQueue",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QueueGuid = table.Column<Guid>(nullable: false),
                    QuestionVersion = table.Column<Guid>(nullable: false),
                    QueueUser = table.Column<Guid>(nullable: false),
                    Ctime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionQueue", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    AnswerGuid = table.Column<Guid>(nullable: false),
                    Userid = table.Column<Guid>(nullable: false),
                    RecordId = table.Column<int>(nullable: false),
                    Ctime = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Question = table.Column<Guid>(nullable: false),
                    QuestionnaireGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.AnswerGuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Questionnaire");

            migrationBuilder.DropTable(
                name: "QuestionQueue");

            migrationBuilder.DropTable(
                name: "Result");
        }
    }
}
