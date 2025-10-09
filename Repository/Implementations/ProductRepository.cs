using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace inventory_management_system.Repository.Implementations
{
    public class ProductRepository : IProductRepository
        {
        private readonly ApplicationDBContext _context;

        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<ProductResponse> CreateProductAsync(ProductDto product)
        {
            var entity = product.MappedProduct();
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
            return new ProductResponse
            {
                Name = entity.Name,
                SKU = entity.SKU,
                UnitPrice = entity.UnitPrice,
                CostPrice = entity.CostPrice,
                SupplierId = entity.SupplierId,
                SupplierName = entity.Supplier?.Name,
                CategoryId = (int)entity.CategoryId,
                CategoryName = entity.Category?.CategoryName,
                ReorderLevel = entity.ReorderLevel,
                MinStock = entity.MinStock,
                MaxStock = entity.MaxStock,
                IsActive = entity.IsActive
            };
        }
        public async Task<(IEnumerable<Product> products, int totalCount)> GetAllProductsAsync(GetAllProductsRequest request)   
        {
            var query = _context.Products
               .Include(i => i.Category)
               .AsQueryable();

            // Filtering
            if (request.CategoryId.HasValue)
                query = query.Where(i => i.CategoryId == request.CategoryId.Value);

            // Total count
            var totalCount = await query.CountAsync();

            // sorting and pagination
            var products = await query
                .OrderBy(i => i.CategoryId)
                .ThenBy(i => i.ProductId)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(u => u.ProductId == id);

            if (product is null)
                throw new KeyNotFoundException("Product Not Found");

            return new ProductResponse
            {
                ProductId = product.ProductId,
                Name = product.Name,
                SKU = product.SKU,
                UnitPrice = product.UnitPrice,
                CostPrice = product.CostPrice,
                SupplierId = product.SupplierId,
                SupplierName = product.Supplier?.Name,
                CategoryId = (int)product.CategoryId,
                CategoryName = product.Category?.CategoryName,
                ReorderLevel = product.ReorderLevel,
                MinStock = product.MinStock,
                MaxStock = product.MaxStock,
                IsActive = product.IsActive
            };
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(u => u.ProductId == id) ?? throw new KeyNotFoundException("Product not found");
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Product> UpdateProductAsync(ProductDto product, int id)
        {
            var entity = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (entity == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            var updatedProduct = product.MappedProduct();
            _context.Products.Update(updatedProduct);
            await _context.SaveChangesAsync();
            return updatedProduct;
        }
       
    }

}
