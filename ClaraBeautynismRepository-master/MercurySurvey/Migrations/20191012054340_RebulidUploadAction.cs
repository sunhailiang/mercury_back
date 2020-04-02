using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MercurySurvey.Migrations
{
    public partial class RebulidUploadAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Files",
                table: "UploadFile");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "UserInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalFilePath",
                table: "UploadFile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Province",
                table: "UserInformation");

            migrationBuilder.DropColumn(
                name: "LocalFilePath",
                table: "UploadFile");

            migrationBuilder.AddColumn<byte[]>(
                name: "Files",
                table: "UploadFile",
                nullable: true);
        }
    }
}
