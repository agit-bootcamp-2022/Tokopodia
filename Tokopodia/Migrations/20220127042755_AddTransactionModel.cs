using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tokopodia.Migrations
{
    public partial class AddTransactionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "BuyerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SumBillingSeller = table.Column<double>(type: "float", nullable: false),
                    SumShippingCost = table.Column<double>(type: "float", nullable: false),
                    TotalBilling = table.Column<double>(type: "float", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WalletTransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_TransactionId",
                table: "Carts",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Transactions_TransactionId",
                table: "Carts",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Transactions_TransactionId",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Carts_TransactionId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "BuyerProfiles");
        }
    }
}
