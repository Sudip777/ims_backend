using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Services.Implementations
{
    public class SupplierService : ISupplierService
    {
        private readonly IUserService _userService;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ApplicationDBContext _context;


        public SupplierService(IUserService userService, ISupplierRepository supplierRepository, ApplicationDBContext context)
        {
            _userService = userService;
            _supplierRepository = supplierRepository;
            _context = context;
        }
        public async Task<SupplierResponse> CreateSupplierAsync(SupplierDto supplier)
        {
            var response = supplier.MappedSupplier();
            response.CreatedByUserId = _userService.GetCurrentUserId();
            var createdResponse = await _supplierRepository.CreateSupplierAsync(response);
            return new SupplierResponse
            {
                SupplierId = createdResponse.SupplierId,
                Name = createdResponse.Name,
                Email = createdResponse.Email,
                Phone = createdResponse.Phone,
                Address = createdResponse.Address,
                IsActive = createdResponse.IsActive,
                CreatedAt = createdResponse.CreatedAt,
                CreatedByUserId = createdResponse.CreatedByUserId,
            };
        }

        public async Task<IEnumerable<SupplierResponse>> GetAllSupplierAsync()
        {
            var res = await _supplierRepository.GetAllSupplierAsync();
            return  res
                .Select(i => new SupplierResponse
                {
                    SupplierId = i.SupplierId,
                    Name = i.Name,
                    Email = i.Email,
                    Phone = i.Phone,
                    Address = i.Address,
                    IsActive = i.IsActive,
                    CreatedAt = i.CreatedAt,
                    CreatedByUserId = i.CreatedByUserId
                })
                .ToList();
        }

        public async Task<SupplierResponse> GetSupplierByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid Supplier ID");
            }

            var data = await _supplierRepository.GetSupplierByIdAsync(id);

            if (data == null)
            {
                throw new KeyNotFoundException($"Supplier with ID {id} not found.");
            }

            return new SupplierResponse
            {
                SupplierId = data.SupplierId,
                Name = data.Name,
                Email = data.Email,
                Phone = data.Phone,
                Address = data.Address,
                IsActive = data.IsActive,
                CreatedAt = data.CreatedAt,
                CreatedByUserId = data.CreatedByUserId
            };
        }

        public async Task<SupplierResponse> UpdateSupplierAsync(int id, SupplierDto supplier)
        {
            var data = await _supplierRepository.GetSupplierByIdAsync(id);
            if (data == null)
            {
                throw new KeyNotFoundException($"Inventory with ID {id} not found.");
            }
            var tempData = await _supplierRepository.UpdateSupplierAsync(supplier, id);

            return SupplierResponse.MappedSupplierResponse(tempData);
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid Supplier ID");
            }

            var user = await _supplierRepository.GetSupplierByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            await _supplierRepository.DeleteSupplierAsync(id);
            return true;
        }

        public async Task EnsureSupplierExistsAsync(int supplierId, bool onlyActive = true)
        {
            var query = _context.Suppliers.AsQueryable();
            if (onlyActive) query = query.Where(p => p.IsActive);

            var exists = await query.AnyAsync(p => p.SupplierId == supplierId);
            if (!exists)
                throw new KeyNotFoundException($"Supplier with ID {supplierId} does not exist.");
        }
    }
}
