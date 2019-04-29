using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class AddDateUpdatedToFileItemEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "FileItem");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "FileItem",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "FileItem");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "FileItem",
                nullable: false,
                defaultValue: 0);
        }
    }
}
