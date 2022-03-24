using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFun.Infrastructure.Data.Migrations
{
    public partial class ChangeRelationActionOnDeleteForProductAndProductCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersProducts_Products_ProductId",
                table: "OrdersProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductsCategories_CategoryId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersProducts_Products_ProductId",
                table: "OrdersProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductsCategories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "ProductsCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersProducts_Products_ProductId",
                table: "OrdersProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductsCategories_CategoryId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersProducts_Products_ProductId",
                table: "OrdersProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductsCategories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "ProductsCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
