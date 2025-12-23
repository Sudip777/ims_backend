using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for role permission repository operations including retrieving, creating, updating, and deleting role permissions.
    /// </summary>
    public interface IRolePermissionRepository
    {
        /// <summary>
        /// Retrieves all role permissions from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all role permissions</returns>
        Task<IEnumerable<RolePermission>> GetAllRolePermissionsAsync();
        /// <summary>
        /// Retrieves a role permission by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the role permission to retrieve</param>
        /// <returns>The requested role permission if found, otherwise null</returns>
        Task<RolePermission> GetRolePermissionByIdAsync(int id);
        /// <summary>
        /// Creates a new role permission in the database asynchronously.
        /// </summary>
        /// <param name="permission">The role permission to create</param>
        /// <returns>The created role permission with updated information</returns>
        Task<RolePermission> CreateRolePermissionAsync(RolePermission permission);
        /// <summary>
        /// Updates an existing role permission in the database asynchronously.
        /// </summary>
        /// <param name="permission">The role permission data to update</param>
        /// <param name="id">The ID of the role permission to update</param>
        /// <returns>The updated role permission</returns>
        Task<RolePermission> UpdateRolePermissionAsync(RolePermissionDto permission, int id);
        /// <summary>
        /// Deletes a role permission from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the role permission to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteRolePermissionAsync(int id);


    }
}
