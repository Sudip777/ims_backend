namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for user menu repository operations including retrieving mapped URLs by role ID.
    /// </summary>
    public interface IUserMenuRepository
    {
        /// <summary>
        /// Retrieves a list of mapped URLs for a specific role ID asynchronously.
        /// </summary>
        /// <param name="roleId">The ID of the role to retrieve mapped URLs for</param>
        /// <returns>A list of mapped URLs for the specified role</returns>
        Task<List<string>> GetMappedUrlsByRoleIdAsync(int roleId);
    }
}
