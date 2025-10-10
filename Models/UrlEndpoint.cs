using System.ComponentModel.DataAnnotations;

namespace inventory_management_system.Models
{
    public class UrlEndpoint
    {
        [Key]
        [Required]
        public int UrlEndpointId { get; set; }
        [Required]
        public required string Url { get; set; }
        public string? Description { get; set; }
    }
}
