using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Requests
{
    public class UrlEndpointDto
    {
  
            public required string Url { get; set; }
            public required string Description { get; set; }


        public UrlEndpoint MappedUrlEndpoint()
        {
            return new UrlEndpoint
            {
                Url = this.Url,
                Description = this.Description
            };
        }

    }
}
