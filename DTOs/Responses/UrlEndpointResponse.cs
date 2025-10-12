using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class UrlEndpointResponse
    {
        public required int UrlEndpointId { get; set; }
        public required string Url { get; set; }
        public required string Description { get; set; }

        public static UrlEndpointResponse MappedUrlEndpointResponse(UrlEndpoint model)
        {
            return new UrlEndpointResponse
            {
                UrlEndpointId = model.UrlEndpointId,
                Url = model.Url,
                Description = model.Description ?? string.Empty,
            };
        }
    }
}
