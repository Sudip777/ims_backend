using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace inventory_management_system.Models
{
    public class Onboarding
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OnboardinigId { get; set; }
        public string? BusinessName { get; set; }
        public string? Domain { get; set; }
        public string? Type { get; set; }
        public string? Identity { get; set; }
        public string? StorageSize { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }


    }
}
