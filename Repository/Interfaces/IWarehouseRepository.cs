using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse);
        Task<IEnumerable<Warehouse>> GetAllWarehousesAsync();
        Task<Warehouse> GetWarehouseByIdAsync(int id);
        Task<Warehouse> UpdateWarehouseAsync(WarehouseDto warehouse, int id);
        Task<Warehouse?> GetByNameAsync(string name);
    }
}
