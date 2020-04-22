using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class Release01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "CurrentUserSubscription");

            migrationBuilder.AddColumn<long>(
                name: "Ticks",
                table: "CurrentUserSubscription",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ticks",
                table: "CurrentUserSubscription");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "CurrentUserSubscription",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
