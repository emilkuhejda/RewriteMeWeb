using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class Release02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WavPartialFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileItemId = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: false),
                    AudioChannels = table.Column<int>(nullable: false),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false),
                    TotalTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WavPartialFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WavPartialFile_FileItem_FileItemId",
                        column: x => x.FileItemId,
                        principalTable: "FileItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WavPartialFile_FileItemId",
                table: "WavPartialFile",
                column: "FileItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WavPartialFile");
        }
    }
}
