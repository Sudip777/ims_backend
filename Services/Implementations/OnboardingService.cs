using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class OnboardingService : IOnboardingService
    {

        private readonly IOnboardingRepository _repository;
        public OnboardingService(IOnboardingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Onboarding> CreateOnboardingAsync(OnboardingDto dto)
        {
            var onboardingEntity = dto.MappedOnboarding();
            if (onboardingEntity == null)
                throw new KeyNotFoundException();

            await _repository.CreateOnboardingAsync(onboardingEntity);
            return onboardingEntity; 
        }
    }
}
