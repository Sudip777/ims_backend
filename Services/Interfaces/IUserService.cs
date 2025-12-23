using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Services.Interfaces
{
    /// <summary>
    /// Interface for user service operations including retrieving, creating, updating, and deleting users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user asynchronously and returns the created user response.
        /// </summary>
        /// <param name="dto">The register user data transfer object containing user information</param>
        /// <returns>The created user response</returns>
        Task<UserResponse> RegisterAsync(RegisterUserDto dto);
        /// <summary>
        /// Retrieves a user by their username asynchronously and returns the user entity.
        /// </summary>
        /// <param name="username">The username of the user to retrieve</param>
        /// <returns>The requested user entity if found, otherwise null</returns>
        Task<User?> GetByUsernameAsync(string username);
        /// <summary>
        /// Retrieves a user by their ID asynchronously and returns the user entity.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve</param>
        /// <returns>The requested user entity if found, otherwise null</returns>
        Task<User?> GetByIdAsync(int id);
        /// <summary>
        /// Updates an existing user asynchronously and returns the updated user response.
        /// </summary>
        /// <param name="userId">The ID of the user to update</param>
        /// <param name="dto">The update user data transfer object containing updated user information</param>
        /// <returns>The updated user response</returns>
        Task<UserResponse> UpdateUserAsync(int userId, UpdateUserDto dto);
        /// <summary>
        /// Deletes a user asynchronously by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteUserAsync(int userId);
        /// <summary>
        /// Retrieves the current user asynchronously and returns the user response.
        /// </summary>
        /// <param name="userId">The ID of the current user to retrieve</param>
        /// <returns>The current user response if found, otherwise null</returns>
        Task<UserResponse?> GetCurrentUserAsync(int userId);
        /// <summary>
        /// Validates user login asynchronously and returns the user entity if valid.
        /// </summary>
        /// <param name="loginDto">The login data transfer object containing login credentials</param>
        /// <returns>The user entity if login is valid, otherwise null</returns>
        Task<User?> ValidateLoginAsync(LoginDto loginDto);
        /// <summary>
        /// Validates a user with refresh token asynchronously and returns the user entity if valid.
        /// </summary>
        /// <param name="userId">The ID of the user to validate</param>
        /// <returns>The user entity if validation is successful, otherwise null</returns>
        Task<User?> ValidateRefreshTokenUserAsync(int userId);
        /// <summary>
        /// Gets the current user ID from the authentication context.
        /// </summary>
        /// <returns>The ID of the current user</returns>
        int GetCurrentUserId();
    }
}
