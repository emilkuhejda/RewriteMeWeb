using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class AddLanguageColumnToFileItemEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "FileItem",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "FileItem");
        }
    }
}
