using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateInfomationMessageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "InformationMessage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InformationMessage_UserId",
                table: "InformationMessage",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InformationMessage_User_UserId",
                table: "InformationMessage",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InformationMessage_User_UserId",
                table: "InformationMessage");

            migrationBuilder.DropIndex(
                name: "IX_InformationMessage_UserId",
                table: "InformationMessage");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InformationMessage");
        }
    }
}
