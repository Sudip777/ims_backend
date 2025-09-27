using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> RegisterAsync(RegisterUserDto dto);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task<UserResponse> UpdateUserAsync(int userId, UpdateUserDto dto);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserResponse?> GetCurrentUserAsync(int userId);
        Task<User?> ValidateLoginAsync(LoginDto loginDto);
        Task<User?> ValidateRefreshTokenUserAsync(int userId);
    }
}
