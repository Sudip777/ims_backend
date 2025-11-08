using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Services.Interfaces
{
    public interface IOnboardingService
    {
        Task<Onboarding> CreateOnboardingAsync(OnboardingDto dto);
    }
}
