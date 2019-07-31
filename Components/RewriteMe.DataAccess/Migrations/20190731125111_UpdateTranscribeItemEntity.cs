using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateTranscribeItemEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "TranscribeItem");

            migrationBuilder.AddColumn<string>(
                name: "SourceFileName",
                table: "TranscribeItem",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceFileName",
                table: "TranscribeItem");

            migrationBuilder.AddColumn<byte[]>(
                name: "Source",
                table: "TranscribeItem",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
