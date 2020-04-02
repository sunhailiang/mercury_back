using Microsoft.EntityFrameworkCore.Migrations;

namespace MercurySurvey.Migrations
{
    public partial class FixedUserHeightDataTypeError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Height",
                table: "UserInformation",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Height",
                table: "UserInformation",
                nullable: true,
                oldClrType: typeof(double));
        }
    }
}
