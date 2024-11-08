using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopEasy.Ims.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IsStockLowcolumnadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStockLow",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStockLow",
                table: "Products");
        }
    }
}
