using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Security;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
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
            if (id <= 0)
            {
                throw new ArgumentException("Invalid User ID");
            }
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
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid User ID");
            }
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<UserResponse> UpdateUserAsync(int userId, UpdateUserDto dto)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid User ID");
            }
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
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid User ID");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            if (user.Role?.RoleName == "ADMIN")
            {
              
                var adminCount = await _userRepository.CountAdminsAsync();
                if (adminCount == 1)
                    throw new InvalidOperationException("Cannot delete the last Admin user.");
            }
            await _userRepository.DeleteAsync(userId);
            return true;
        }

        public  int GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException();

            var claim = user.Claims.FirstOrDefault(c => c.Type == "UserId")
                        ?? throw new UnauthorizedAccessException("User ID claim missing");

            if (!int.TryParse(claim.Value, out int userId))
                throw new UnauthorizedAccessException("User ID claim invalid");

            return userId;
        }


    }
}
