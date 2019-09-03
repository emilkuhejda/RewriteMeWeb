using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateInformationMessageEntityInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "InformationMessage");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "InformationMessage");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "InformationMessage");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "InformationMessage");

            migrationBuilder.CreateTable(
                name: "LanguageVersion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InformationMessageId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Message = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Language = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageVersion_InformationMessage_InformationMessageId",
                        column: x => x.InformationMessageId,
                        principalTable: "InformationMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageVersion_InformationMessageId",
                table: "LanguageVersion",
                column: "InformationMessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageVersion");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "InformationMessage",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "InformationMessage",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "InformationMessage",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "InformationMessage",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
