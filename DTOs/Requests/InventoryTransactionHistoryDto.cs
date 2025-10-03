using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.DTOs.Requests
{
    public class InventoryTransactionHistoryDto
    {

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }
        public int? OrderId { get; set; }
        public int? PurchaseOrderId { get; set; } = null;

        [Required]
        public int UserId { get; set; }
        public int QuantityChange { get; set; }
        [StringLength(255)]
        public string? Details { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;



        public InventoryTransactionHistory MappedInventoryTransactionHistory()
        {
            return new InventoryTransactionHistory
            {
                ProductId = this.ProductId,
                WarehouseId = this.WarehouseId,
                TransactionTypeId = this.TransactionTypeId,
                OrderId = this.OrderId,
                PurchaseOrderId = this.PurchaseOrderId,
                UserId = this.UserId,
                QuantityChange = this.QuantityChange,
                Details = this.Details
            };
    }
    }

}