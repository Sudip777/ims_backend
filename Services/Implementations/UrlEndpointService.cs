using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class UrlEndpointService : IUrlEndpointService
    {
        private readonly IUrlEndpointRepository _urlEndpointRepository;
        public UrlEndpointService(IUrlEndpointRepository urlEndpointRepository)
        {
            _urlEndpointRepository = urlEndpointRepository;
        }


        public async Task<IEnumerable<UrlEndpointResponse>> GetAllUrlEndpointsAsync()
        {
            var res = await _urlEndpointRepository.GetAllUrlEndpointsAsync();
            if (res == null) throw new Exception("Url Endpoints Not Found.");
            return res
                .Select(response => new UrlEndpointResponse
                {
                    UrlEndpointId = response.UrlEndpointId,
                    Url = response.Url,
                    Description = response.Description ?? string.Empty,
                })
                .ToList();
        }

        public async Task<UrlEndpointResponse> GetUrlEndpointByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid Url Endpoint ID");

            var response = await _urlEndpointRepository.GetUrlEndpointByIdAsync(id);

            return new UrlEndpointResponse
            {
                UrlEndpointId = response.UrlEndpointId,
                Url = response.Url,
                Description = response.Description ?? string.Empty,
            };
        }
        public async Task<UrlEndpointResponse> CreateUrlEndpointAsync(UrlEndpointDto dto)
        {
            var response = dto.MappedUrlEndpoint();
            var createdResponse = await _urlEndpointRepository.CreateUrlEndpointAsync(response);
           return new UrlEndpointResponse
            {
                UrlEndpointId = response.UrlEndpointId,
                Url = response.Url,
                Description = response.Description ?? string.Empty,
            };
        }

        public async Task<UrlEndpointResponse> UpdateUrlEndpointAsync(UrlEndpointDto dto, int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid Url Endpoint ID");

            var data = await _urlEndpointRepository.GetUrlEndpointByIdAsync(id);
            if (data == null)
            {
                throw new KeyNotFoundException($"Url Endpoint with ID {id} Not Found.");
            }
            var tempData = await _urlEndpointRepository.UpdateUrlEndpointAsync(dto, id);

            return UrlEndpointResponse.MappedUrlEndpointResponse(tempData);
        }

        public async Task<bool> DeleteUrlEndpointAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid Url Endpoint ID");

            var data = await GetUrlEndpointByIdAsync(id);

            if(data == null) throw new KeyNotFoundException($"Url Endpoint with ID: {id} Not Found.");

            return await _urlEndpointRepository.DeleteUrlEndpointAsync(id);
        }
    }
}
