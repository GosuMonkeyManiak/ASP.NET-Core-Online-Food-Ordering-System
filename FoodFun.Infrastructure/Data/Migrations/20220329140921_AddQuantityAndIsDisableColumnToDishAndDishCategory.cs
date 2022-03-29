using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFun.Infrastructure.Data.Migrations
{
    public partial class AddQuantityAndIsDisableColumnToDishAndDishCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_DishesCategories_CategoryId",
                table: "Dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDishes_Dishes_DishId",
                table: "OrdersDishes");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisable",
                table: "DishesCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "Quantity",
                table: "Dishes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_DishesCategories_CategoryId",
                table: "Dishes",
                column: "CategoryId",
                principalTable: "DishesCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDishes_Dishes_DishId",
                table: "OrdersDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_DishesCategories_CategoryId",
                table: "Dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDishes_Dishes_DishId",
                table: "OrdersDishes");

            migrationBuilder.DropColumn(
                name: "IsDisable",
                table: "DishesCategories");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Dishes");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_DishesCategories_CategoryId",
                table: "Dishes",
                column: "CategoryId",
                principalTable: "DishesCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDishes_Dishes_DishId",
                table: "OrdersDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
