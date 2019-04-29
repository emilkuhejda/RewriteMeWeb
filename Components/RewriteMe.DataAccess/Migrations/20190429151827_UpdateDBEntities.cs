using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateDBEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "TranscribeItem");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "TranscribeItem",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AudioSourceVersion",
                table: "FileItem",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "TranscribeItem");

            migrationBuilder.DropColumn(
                name: "AudioSourceVersion",
                table: "FileItem");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "TranscribeItem",
                nullable: false,
                defaultValue: 0);
        }
    }
}
