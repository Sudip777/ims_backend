using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;

namespace inventory_management_system.DTOs.Responses
{
    public class InventoryTransactionHistoryResponse
    {
        public int TransactionId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public int QuantityChange { get; set; }

        [Required]
        public int TransactionTypeId { get; set; } = 0;
        public int? OrderId { get; set; }
        public int? PurchaseOrderId { get; set; }

        [StringLength(255)]
        public string? Details { get; set; }

        [Required]
        public int UserId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;





        public InventoryTransactionHistoryResponse MappedInventoryTransactionHistoryResponse(InventoryTransactionHistoryResponse res)
        {
            return new InventoryTransactionHistoryResponse
            {
                TransactionId = this.TransactionId,
                ProductId = this.ProductId,
                WarehouseId = this.WarehouseId,
                QuantityChange = this.QuantityChange,
                TransactionTypeId = this.TransactionTypeId,
                OrderId = this.OrderId,
                PurchaseOrderId = this.PurchaseOrderId,
                Details = this.Details,
                UserId = this.UserId,
                TransactionDate = this.TransactionDate,
            };

        }
    }
}
