namespace inventory_management_system.DTOs.Requests
{
    public class InventoryTransactionOverviewDto
    {
       public  string SummaryCategory { get; set; }
        public string Name { get; set; }
        public int? TotalTransactions { get; set; }
        public int? ProductId { get; set; }
        public int? QuantityMetric { get; set; }



    }
}
