using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    public interface IWarehouseService
    {
        Task<IEnumerable<WarehouseResponse>> GetAllWarehousesAsync();
        Task<WarehouseResponse> GetWarehouseByIdAsync(int id);
        Task<WarehouseResponse> CreateWarehouseAsync(WarehouseDto warehouse);
        Task<WarehouseResponse> UpdateWarehouseAsync(WarehouseDto warehouse, int id);
    }
}
