using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    public interface IUrlEndpointService
    {
        Task<IEnumerable<UrlEndpointResponse>> GetAllUrlEndpointsAsync();
        Task<UrlEndpointResponse> GetUrlEndpointByIdAsync(int id);
        Task<UrlEndpointResponse> CreateUrlEndpointAsync(UrlEndpointDto dto);
        Task<UrlEndpointResponse> UpdateUrlEndpointAsync(UrlEndpointDto dto, int id);
        Task<bool> DeleteUrlEndpointAsync(int id);
    }
}
