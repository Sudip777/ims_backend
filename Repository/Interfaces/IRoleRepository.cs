using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for role repository operations including retrieving and creating roles.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Creates a new role in the database asynchronously.
        /// </summary>
        /// <param name="role">The role to create</param>
        /// <returns>The created role with updated information</returns>
        Task<Role> CreateAsync(Role role);
        /// <summary>
        /// Retrieves all roles from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all roles as role responses</returns>
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync();

    }
}
