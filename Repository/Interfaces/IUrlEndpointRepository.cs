using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IUrlEndpointRepository
    {
        Task<IEnumerable<UrlEndpoint>> GetAllUrlEndpointsAsync();
        Task<UrlEndpoint> GetUrlEndpointByIdAsync(int id);
        Task<UrlEndpoint> CreateUrlEndpointAsync(UrlEndpoint urlEndpoint);
        Task<UrlEndpoint> UpdateUrlEndpointAsync(UrlEndpointDto urlEndpoint, int id);
        Task<bool> DeleteUrlEndpointAsync(int id);
    }
}
