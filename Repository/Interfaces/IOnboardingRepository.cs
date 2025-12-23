using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for onboarding repository operations including creating onboarding records.
    /// </summary>
    public interface IOnboardingRepository
    {
        /// <summary>
        /// Creates a new onboarding record in the database asynchronously.
        /// </summary>
        /// <param name="onboarding">The onboarding record to create</param>
        Task CreateOnboardingAsync(Onboarding onboarding);
    }
}
