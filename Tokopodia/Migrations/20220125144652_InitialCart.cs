using Microsoft.EntityFrameworkCore.Migrations;

namespace Tokopodia.Migrations
{
    public partial class InitialCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BillingSeller = table.Column<float>(type: "real", nullable: false),
                    LatSeller = table.Column<double>(type: "float", nullable: false),
                    LongSeller = table.Column<double>(type: "float", nullable: false),
                    LatBuyer = table.Column<double>(type: "float", nullable: false),
                    LongBuyer = table.Column<double>(type: "float", nullable: false),
                    ShippingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingCost = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");
        }
    }
}
