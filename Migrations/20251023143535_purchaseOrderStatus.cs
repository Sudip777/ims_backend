using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventory_management_system.Migrations
{
    /// <inheritdoc />
    public partial class purchaseOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderStatus_StatusId",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderStatus",
                table: "PurchaseOrderStatus");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderStatus",
                newName: "PurchaseOrderStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderStatuses",
                table: "PurchaseOrderStatuses",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderStatuses_StatusId",
                table: "PurchaseOrders",
                column: "StatusId",
                principalTable: "PurchaseOrderStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderStatuses_StatusId",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderStatuses",
                table: "PurchaseOrderStatuses");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderStatuses",
                newName: "PurchaseOrderStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderStatus",
                table: "PurchaseOrderStatus",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderStatus_StatusId",
                table: "PurchaseOrders",
                column: "StatusId",
                principalTable: "PurchaseOrderStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
