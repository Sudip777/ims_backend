using inventory_management_system.Data;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                 .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                 .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);

        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (user.Role?.RoleName == "ADMIN")
            {
                var adminCount = await _context.Users
                    .CountAsync(u => u.Role.RoleName == "ADMIN" && u.IsActive && u.UserId != userId);
                if (adminCount == 0)
                {
                    throw new InvalidOperationException("Cannot delete the last Admin user.");
                }
            }
            // Soft delete
            user.IsActive = false;
            user.LastLogin = DateTime.UtcNow; // Optional: Update last activity timestamp
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return (user);
        }

        public async Task<int> CountAdminsAsync()
        {
            var adminCount = await _context.Users
                .CountAsync(u => u.Role.RoleName == "ADMIN" && u.IsActive);
            return (adminCount);
        }
    }
}
