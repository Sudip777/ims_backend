using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Requests
{
    public class OnboardingDto
    {
        public string? BusinessName { get; set; }
        public string? Domain { get; set; }
        public string? Type { get; set; }
        public string? Identity { get; set; }
        public string? StorageSize { get; set; }

        public Onboarding MappedOnboarding()
        {


            return new Onboarding
            {
                 BusinessName= this.BusinessName,
                Domain = this.Domain,
                Type = this.Type,
                Identity = this.Identity,
                StorageSize = this.StorageSize
            };

        }
    }
}
