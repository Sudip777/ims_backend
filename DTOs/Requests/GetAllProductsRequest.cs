namespace inventory_management_system.DTOs.Requests
{
    public class GetAllProductsRequest
    {
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }    
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
