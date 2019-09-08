using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateDBModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "InformationMessage",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WasOpened",
                table: "InformationMessage",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "InformationMessage");

            migrationBuilder.DropColumn(
                name: "WasOpened",
                table: "InformationMessage");
        }
    }
}
