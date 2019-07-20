using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateDatabaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "RecognizedAudioSample",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RecognizedAudioSample_UserId",
                table: "RecognizedAudioSample",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecognizedAudioSample_User_UserId",
                table: "RecognizedAudioSample",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecognizedAudioSample_User_UserId",
                table: "RecognizedAudioSample");

            migrationBuilder.DropIndex(
                name: "IX_RecognizedAudioSample_UserId",
                table: "RecognizedAudioSample");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RecognizedAudioSample");
        }
    }
}
