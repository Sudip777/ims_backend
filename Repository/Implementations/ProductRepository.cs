using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Extensions;
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
        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<(IEnumerable<Product> products, int totalCount)> GetAllProductsAsync(GetAllProductsRequest request)
        {
            var products = await _context.Products
                .FromSqlInterpolated($@"
                 EXEC dbo.GetProductData
                @CategoryId = {request.CategoryId},
                @SupplierId = {request.SupplierId},
                @Search = {request.Search},
                @SortColumn = {request.SortColumn},
                @SortDirection = {request.SortDirection},
                @Page = {request.Page},
                @PageSize = {request.PageSize}")
                .AsNoTracking()
                .ToListAsync();

            int totalCount = products.Count;
            return (products, totalCount);
        }


        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(u => u.ProductId == id);

            if (product is null)
                throw new KeyNotFoundException("Product Not Found");

            return product;
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(u => u.ProductId == id) ?? throw new KeyNotFoundException("Product not found");
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Product> UpdateProductAsync(ProductDto dto, int id)
        {
            
            var entity = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (entity == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            // DTO to the existing entity
            entity.Name = dto.Name;
            entity.SKU = dto.SKU;
            entity.SupplierId = dto.SupplierId;
            entity.CategoryId = dto.CategoryId;
            entity.UnitPrice = dto.UnitPrice;
            entity.CostPrice = dto.CostPrice;
            entity.ReorderLevel = dto.ReorderLevel;
            entity.MinStock = dto.MinStock;
            entity.MaxStock = dto.MaxStock;
            entity.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return entity;
        }
    }

}

