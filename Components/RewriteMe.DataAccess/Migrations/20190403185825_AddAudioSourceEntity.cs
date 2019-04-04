using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class AddAudioSourceEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "FileItem");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "FileItem");

            migrationBuilder.CreateTable(
                name: "AudioSource",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileItemId = table.Column<Guid>(nullable: false),
                    OriginalSource = table.Column<byte[]>(nullable: false),
                    WavSource = table.Column<byte[]>(nullable: true),
                    ContentType = table.Column<string>(maxLength: 50, nullable: false),
                    TotalTime = table.Column<TimeSpan>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioSource");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "FileItem",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Source",
                table: "FileItem",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
