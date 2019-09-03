using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateDatabaseEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentOnAndroid",
                table: "InformationMessage");

            migrationBuilder.DropColumn(
                name: "SentOnOsx",
                table: "InformationMessage");

            migrationBuilder.AddColumn<bool>(
                name: "SentOnAndroid",
                table: "LanguageVersion",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SentOnOsx",
                table: "LanguageVersion",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentOnAndroid",
                table: "LanguageVersion");

            migrationBuilder.DropColumn(
                name: "SentOnOsx",
                table: "LanguageVersion");

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
    }
}
