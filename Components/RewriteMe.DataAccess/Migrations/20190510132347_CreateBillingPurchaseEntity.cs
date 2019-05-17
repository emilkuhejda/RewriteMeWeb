using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class CreateBillingPurchaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillingPurchase",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    PurchaseId = table.Column<string>(nullable: false),
                    ProductId = table.Column<string>(maxLength: 100, nullable: false),
                    AutoRenewing = table.Column<string>(nullable: false),
                    PurchaseState = table.Column<string>(maxLength: 100, nullable: false),
                    ConsumptionState = table.Column<string>(maxLength: 100, nullable: false),
                    TransactionDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingPurchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingPurchase_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillingPurchase_UserId",
                table: "BillingPurchase",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillingPurchase");
        }
    }
}
