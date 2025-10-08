namespace inventory_management_system.DTOs.Requests
{
   
        public class InventoryOverviewDto
        {
            public int TotalProductsInStock { get; set; }
            public decimal TotalStockValue { get; set; }
            public int LowStockProducts { get; set; }
            public int OutOfStockProducts { get; set; }
        }

}
