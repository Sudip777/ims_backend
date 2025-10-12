using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
namespace inventory_management_system.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDBContext _context;

        public ProductService(IProductRepository productRepository, ApplicationDBContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<ProductResponse> RegisterProductAsync(ProductDto dto)
        {
            var response = dto.MappedProduct();

            var newProduct = await _productRepository.CreateProductAsync(response);
            return new ProductResponse
            {

                ProductId = newProduct.ProductId,
                Name = newProduct.Name,
                SKU = newProduct.SKU,
                UnitPrice = newProduct.UnitPrice,
                CostPrice = newProduct.CostPrice,
                SupplierId = newProduct.SupplierId,
                SupplierName = newProduct.Supplier?.Name,
                CategoryId = (int)newProduct.CategoryId,
                CategoryName = newProduct.Category?.CategoryName,
                ReorderLevel = newProduct.ReorderLevel,
                MinStock = newProduct.MinStock,
                MaxStock = newProduct.MaxStock,
                IsActive = newProduct.IsActive,
                CreatedAt = newProduct.CreatedAt,
            }; 
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Product ID must be greater than zero.", nameof(id));

            var res = await _productRepository.GetProductByIdAsync(id);
            return new ProductResponse
            {
                ProductId = res.ProductId,
                Name = res.Name,
                SKU = res.SKU,
                UnitPrice = res.UnitPrice,
                CostPrice = res.CostPrice,
                SupplierId = res.SupplierId,
                SupplierName = res.Supplier?.Name,
                CategoryId = (int)res.CategoryId,
                CategoryName = res.Category?.CategoryName,
                ReorderLevel = res.ReorderLevel,
                MinStock = res.MinStock,
                MaxStock = res.MaxStock,
                IsActive = res.IsActive
            };
        }

        public async Task<PagedResponse<ProductResponse>> GetAllProductsAsync(GetAllProductsRequest req)
        {
            // Validate pagination parameters
            if (req.Page < 1) req.Page = 1;
            if (req.PageSize < 1 || req.PageSize > 100) req.PageSize = 10;
            var (products, totalCount) = await _productRepository.GetAllProductsAsync(req);

            var responses = products.Select(ProductResponse.MappeddProductResponse).ToList();
            return new PagedResponse<ProductResponse>
            {
                Data = responses,
                Meta = new PagedResponse<ProductResponse>.MetaData
                {
                    TotalCount = totalCount,
                    Page = req.Page,
                    PageSize = req.PageSize
                }
            };
        }

        public Task<bool> DeleteProductAsync(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("Order ID must be greater than zero.", nameof(productId));

            return _productRepository.DeleteProductAsync(productId);
        }

        public async Task<ProductResponse> UpdateProductAsync(int productId, ProductDto dto)
        {
            if (productId <= 0)
                throw new ArgumentException("Order ID must be greater than zero.", nameof(productId));

            var productResponse = await _productRepository.GetProductByIdAsync(productId);
            if (productResponse == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
            var updatedProduct = await _productRepository.UpdateProductAsync(dto, productId);

            return ProductResponse.MappeddProductResponse(updatedProduct);
        }

          

    }
}
