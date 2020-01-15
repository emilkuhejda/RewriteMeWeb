using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateDBModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploadedChunk",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileItemId = table.Column<Guid>(nullable: false),
                    Source = table.Column<byte[]>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedChunk", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadedChunk");
        }
    }
}
