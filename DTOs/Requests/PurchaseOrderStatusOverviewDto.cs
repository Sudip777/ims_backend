namespace inventory_management_system.DTOs.Requests
{
    public class PurchaseOrderStatusOverviewDto
    {
        public int PendingOrdersCount { get; set; }
        public int ReceivedOrdersCount { get; set; }
        public int CancelledOrdersCount { get; set; }
        public decimal TotalPendingOrderValue { get; set; }
    }
}
