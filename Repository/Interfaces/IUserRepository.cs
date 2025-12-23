using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for user repository operations including retrieving, creating, updating, and deleting users.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their username asynchronously.
        /// </summary>
        /// <param name="username">The username of the user to retrieve</param>
        /// <returns>The requested user if found, otherwise null</returns>
        Task<User?> GetByUsernameAsync(string username);
        /// <summary>
        /// Retrieves a user by their email asynchronously.
        /// </summary>
        /// <param name="email">The email of the user to retrieve</param>
        /// <returns>The requested user if found, otherwise null</returns>
        Task<User?> GetByEmailAsync(string email);
        /// <summary>
        /// Retrieves a user by their ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve</param>
        /// <returns>The requested user if found, otherwise null</returns>
        Task<User?> GetByIdAsync(int id);
        /// <summary>
        /// Creates a new user in the database asynchronously.
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <returns>The created user with updated information</returns>
        Task<User> CreateAsync(User user);
        /// <summary>
        /// Updates an existing user in the database asynchronously.
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <returns>The updated user</returns>
        Task<User> UpdateAsync(User user);
        /// <summary>
        /// Deletes a user from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user to delete</param>
        /// <returns>The deleted user</returns>
        Task<User> DeleteAsync(int id);

        /// <summary>
        /// Counts the number of admin users in the database asynchronously.
        /// </summary>
        /// <returns>The count of admin users</returns>
        Task<int> CountAdminsAsync();
    }
}
