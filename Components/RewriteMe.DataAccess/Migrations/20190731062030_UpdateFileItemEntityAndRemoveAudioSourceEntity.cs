using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateFileItemEntityAndRemoveAudioSourceEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioSource");

            migrationBuilder.AddColumn<string>(
                name: "OriginalContentType",
                table: "FileItem",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalSourceFileName",
                table: "FileItem",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceFileName",
                table: "FileItem",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalContentType",
                table: "FileItem");

            migrationBuilder.DropColumn(
                name: "OriginalSourceFileName",
                table: "FileItem");

            migrationBuilder.DropColumn(
                name: "SourceFileName",
                table: "FileItem");

            migrationBuilder.CreateTable(
                name: "AudioSource",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentType = table.Column<string>(maxLength: 50, nullable: false),
                    FileItemId = table.Column<Guid>(nullable: false),
                    OriginalSource = table.Column<byte[]>(nullable: false),
                    TotalTime = table.Column<TimeSpan>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    WavSource = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AudioSource_FileItem_FileItemId",
                        column: x => x.FileItemId,
                        principalTable: "FileItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioSource_FileItemId",
                table: "AudioSource",
                column: "FileItemId",
                unique: true);
        }
    }
}
