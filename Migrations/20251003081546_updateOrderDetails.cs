using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventory_management_system.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add WarehouseId column to OrderDetails
            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0); // defaultValue ensures existing rows have a value; you may need to adjust

            // Create foreign key constraint to Warehouses table
            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Warehouses_WarehouseId",
                table: "OrderDetails",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            // Optionally, create an index for faster joins
            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_WarehouseId",
                table: "OrderDetails",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key first
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Warehouses_WarehouseId",
                table: "OrderDetails");

            // Drop index
            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_WarehouseId",
                table: "OrderDetails");

            // Drop column
            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "OrderDetails");
        }
    }
}
