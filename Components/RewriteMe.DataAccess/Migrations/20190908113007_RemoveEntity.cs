using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class RemoveEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpenedMessage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OpenedMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InformationMessageId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenedMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenedMessage_InformationMessage_InformationMessageId",
                        column: x => x.InformationMessageId,
                        principalTable: "InformationMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpenedMessage_InformationMessageId",
                table: "OpenedMessage",
                column: "InformationMessageId");
        }
    }
}
