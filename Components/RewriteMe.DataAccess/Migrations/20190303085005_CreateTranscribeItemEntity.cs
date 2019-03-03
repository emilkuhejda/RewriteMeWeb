using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class CreateTranscribeItemEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TranscribeItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileItemId = table.Column<Guid>(nullable: false),
                    Transcript = table.Column<string>(nullable: false),
                    Source = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranscribeItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranscribeItem_FileItem_FileItemId",
                        column: x => x.FileItemId,
                        principalTable: "FileItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TranscribeItem_FileItemId",
                table: "TranscribeItem",
                column: "FileItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TranscribeItem");
        }
    }
}
