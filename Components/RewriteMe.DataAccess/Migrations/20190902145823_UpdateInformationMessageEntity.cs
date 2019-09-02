using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateInformationMessageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InformationMessages");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "InformationMessage",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SentOnAndroid",
                table: "InformationMessage",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SentOnOsx",
                table: "InformationMessage",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "InformationMessage");

            migrationBuilder.DropColumn(
                name: "SentOnAndroid",
                table: "InformationMessage");

            migrationBuilder.DropColumn(
                name: "SentOnOsx",
                table: "InformationMessage");

            migrationBuilder.CreateTable(
                name: "InformationMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CampaignName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationMessages", x => x.Id);
                });
        }
    }
}
