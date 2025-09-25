using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
    }
}
