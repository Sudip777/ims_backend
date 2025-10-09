using Azure.Core;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Implementations;
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
                var newProduct = await _productRepository.CreateProductAsync(dto);
                return newProduct;
           
        }

        public Task<ProductResponse> GetProductByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Product ID must be greater than zero.", nameof(id));

            return _productRepository.GetProductByIdAsync(id);
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
