using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Helpers;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> RegisterAsync(RegisterUserDto dto)
        {
            // Check if username exists
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists.");

            // Check if email exists
            var existingEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                throw new InvalidOperationException("Email already exists.");

            // Create user
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
    }
}
