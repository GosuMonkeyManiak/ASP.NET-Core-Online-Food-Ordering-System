using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFun.Infrastructure.Data.Migrations
{
    public partial class AddIsDisableColumnToProductCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisable",
                table: "ProductsCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisable",
                table: "ProductsCategories");
        }
    }
}
