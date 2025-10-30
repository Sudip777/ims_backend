namespace inventory_management_system.DTOs.Requests
{
    public class GetAllProductsRequest
    {
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public string? Search { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; } = "asc";
        public required int Page { get; set; } = 1;
        public required int PageSize { get; set; } = 10;
    }
}
