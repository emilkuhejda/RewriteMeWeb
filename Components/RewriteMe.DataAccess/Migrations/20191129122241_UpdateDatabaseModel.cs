using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class UpdateDatabaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SourceFileName",
                table: "TranscribeItem",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SourceFileName",
                table: "TranscribeItem",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
