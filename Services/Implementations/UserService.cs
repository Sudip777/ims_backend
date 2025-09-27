using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Helpers;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> RegisterAsync(RegisterUserDto dto)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists.");

            var existingEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                throw new InvalidOperationException("Email already exists.");

            var user = dto.MappedUser();
            user.PasswordHash = PasswordHasher.HashPassword(dto.Password);

            var createdUser = await _userRepository.CreateAsync(user);
            return UserResponse.FromUser(createdUser);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
        public async Task<UserResponse?> GetCurrentUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user != null ? UserResponse.FromUser(user) : null;
        }
        public async Task<User?> ValidateLoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);

            if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, loginDto.Password))
                return null;

            return user;
        }

        public async Task<User?> ValidateRefreshTokenUserAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<UserResponse> UpdateUserAsync(int userId, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            if (user.Username != dto.Username)
            {
                var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
                if (existingUser != null && existingUser.UserId != userId)
                    throw new InvalidOperationException("Username already exists.");
            }
            if (user.Email != dto.Email)
            {
                var existingEmail = await _userRepository.GetByEmailAsync(dto.Email);
                if (existingEmail != null && existingEmail.UserId != userId)
                    throw new InvalidOperationException("Email already exists.");
            }
            user.Username = dto.Username;
            user.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = PasswordHasher.HashPassword(dto.Password);
            }
            user.RoleId = dto.RoleId;
            user.IsActive = dto.IsActive;
            var updatedUser = await _userRepository.UpdateAsync(user);
            return UserResponse.FromUser(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            if (user.Role?.RoleName == "ADMIN")
            {
                // You need a way to count the number of Admin users.
                // This requires a new method in IUserRepository, e.g., Task<int> CountAdminsAsync();
                // For now, let's assume you have added this method to IUserRepository and its implementation.
                var adminCount = await _userRepository.CountAdminsAsync();
                if (adminCount == 1)
                    throw new InvalidOperationException("Cannot delete the last Admin user.");
            }
            await _userRepository.DeleteAsync(userId);
            return true;
        }

       
    }
}
