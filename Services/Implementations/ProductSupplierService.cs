using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Services.Implementations
{
    public class ProductSupplierService : IProductSupplierService

    {
        private readonly IProductSupplierRepository _productSupplierRepository;
        private readonly ApplicationDBContext _context;

        public ProductSupplierService(IProductSupplierRepository productSupplierRepository, ApplicationDBContext context)
        {
            _productSupplierRepository = productSupplierRepository;
            _context = context;
          
        }
        public async Task<ProductSupplierResponse> CreateProductSupplierAsync(ProductSupplierDto dto)
        {
            // 1. Validate ProductId exists
            var productExists = await _context.Products
                .AnyAsync(p => p.ProductId == dto.ProductId);
            if (!productExists)
                throw new KeyNotFoundException($"Product with ID {dto.ProductId} not found.");

            // 2. Validate SupplierId exists
            var supplierExists = await _context.Suppliers
                .AnyAsync(s => s.SupplierId == dto.SupplierId);

            if (!supplierExists)
                throw new KeyNotFoundException($"Supplier with ID {dto.SupplierId} not found.");
            var entity = dto.MappedProductSupplier();
            var res = await _productSupplierRepository.CreateProductSupplierAsync(entity);
            return new ProductSupplierResponse
            {
                ProductSupplierId =entity.ProductSupplierId,
                ProductId =entity.ProductId,
                SupplierId =entity.SupplierId,
                CostPrice =entity.CostPrice,
                CreatedAt =entity.CreatedAt
            };
        }

        public async Task<IEnumerable<ProductSupplierResponse>> GetAllProductSuppliersAsync()
        {
            var suppliers = await _productSupplierRepository.GetAllProductSuppliersAsync();

            return suppliers.Select(ps => new ProductSupplierResponse
            {
                ProductSupplierId = ps.ProductSupplierId,
                ProductId = ps.ProductId,
                SupplierId = ps.SupplierId,
                CostPrice = ps.CostPrice,
                CreatedAt = ps.CreatedAt
            }).ToList();
        }

        

        public async Task<ProductSupplierResponse> GetProductSupplierByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Product Supplier ID must be greater than zero.", nameof(id));

            var suppliers = await _productSupplierRepository.GetAllProductSuppliersAsync();

            var ps = suppliers.FirstOrDefault();
            if (ps == null)
                throw new KeyNotFoundException($" Product Supplier with ID {id} not found.");

            return new ProductSupplierResponse
            {
                ProductSupplierId = ps.ProductSupplierId,
                ProductId = ps.ProductId,
                SupplierId = ps.SupplierId,
                CostPrice = ps.CostPrice,
                CreatedAt = ps.CreatedAt
            };
        }
    }
}
