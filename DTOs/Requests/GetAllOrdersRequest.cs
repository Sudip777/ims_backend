namespace inventory_management_system.DTOs.Requests
{
    public class GetAllOrdersRequest
    {
        public int? CustomerId { get; set; }
        public string? Search { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
