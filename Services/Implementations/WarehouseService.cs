using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace inventory_management_system.Services.Implementations
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUserService _userService;

        public WarehouseService(IWarehouseRepository warehouseRepository, IUserService userService)
        {
            _warehouseRepository = warehouseRepository;
            _userService = userService;
        }
        public async Task<IEnumerable<WarehouseResponse>> GetAllWarehousesAsync()
        {
            var res = await _warehouseRepository.GetAllWarehousesAsync();
            if (res == null)
                throw new KeyNotFoundException("Warehouse Not Found");

            return res.Select(w => new WarehouseResponse
            {
                WarehouseId = w.WarehouseId,
                Name = w.Name,
                CreatedByUserId = w.CreatedByUserId
            }).ToList();

        }

        public async Task<WarehouseResponse> GetWarehouseByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Warehouse ID must be greater than zero.", nameof(id));

            var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(id);

            if (warehouse == null)
                throw new KeyNotFoundException($"Warehouse with ID {id} not found.");

            return new WarehouseResponse
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                CreatedByUserId = warehouse.CreatedByUserId
            };
        }
        public async Task<WarehouseResponse> CreateWarehouseAsync(WarehouseDto dto)
        {
            var response = dto.MappedWarehouse();
            response.CreatedByUserId = _userService.GetCurrentUserId();

            var existingWarehouse = await _warehouseRepository.GetByNameAsync(response.Name);
            if (existingWarehouse != null)
            {
                throw new ArgumentException( $"Warehouse with name '{response.Name}' already exists." );
            }

            await _warehouseRepository.CreateWarehouseAsync(response);
            return new WarehouseResponse
            {
                WarehouseId = response.WarehouseId,
                Name = response.Name,
                CreatedByUserId = response.CreatedByUserId
            };
        }

      
        public async Task<WarehouseResponse> UpdateWarehouseAsync(WarehouseDto warehouse, int id)
        {
            var data = await _warehouseRepository.GetWarehouseByIdAsync(id);
            if (data == null)
            {
                throw new KeyNotFoundException($"Warehouse with ID {id} not found.");
            }
            var tempData = await _warehouseRepository.UpdateWarehouseAsync(warehouse, id);

            return WarehouseResponse.MappedWarehouseResponse(tempData);
        }
    }
}
