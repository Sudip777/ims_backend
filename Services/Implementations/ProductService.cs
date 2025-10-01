using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
           

            var isPresent = CheckProductAndCategoryId(dto.SupplierId, dto.CategoryId.Value);
            if (await isPresent)
            {
                var newProduct = await _productRepository.CreateProductAsync(dto);
                return newProduct;
            }
            else
            {
                throw new ArgumentException("Supplier and Category Must be added");
            }
        }

        public Task<ProductResponse> GetProductByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Product ID must be greater than zero.", nameof(id));

            return _productRepository.GetProductByIdAsync(id);
        }

        public Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            return _productRepository.GetAllProductsAsync();
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

        public async Task ValidateProduct(ProductDto dto, int productId)
        {
            // UnitPrice > CostPrice
            if (dto.UnitPrice <= dto.CostPrice)
            {
                throw new ArgumentException("UnitPrice must be greater than CostPrice.");
            }

            //MinStock <= ReorderLevel <= MaxStock
            if (dto.MinStock > dto.ReorderLevel || (dto.MaxStock.HasValue && dto.ReorderLevel > dto.MaxStock.Value))
            {
                throw new ArgumentException("ReorderLevel must be between MinStock and MaxStock.");
            }

            //uniqueness
            if (!string.IsNullOrEmpty(dto.SKU))
            {
                var skuExists = await _context.Products
                    .AnyAsync(p => p.SKU == dto.SKU && p.ProductId != productId);
                if (skuExists)
                {
                    throw new ArgumentException($"A product with SKU {dto.SKU} already exists.");
                }
            }
        }

        public async Task<bool> CheckProductAndCategoryId(int supplierId, int categoryId)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);
            var category = await _context.Categories.FindAsync(categoryId);

            // true if both exist, false otherwise
            return supplier != null && category != null;
        }


    }
}
