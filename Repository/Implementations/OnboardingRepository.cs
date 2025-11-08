using inventory_management_system.Data;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class OnboardingRepository : IOnboardingRepository
    {
        private readonly ApplicationDBContext _context;

        public OnboardingRepository (ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task CreateOnboardingAsync(Onboarding onboarding)
        {
            
            await _context.Onboardings.AddAsync(onboarding);
            await _context.SaveChangesAsync();
            return;
        }

        
    }
}
