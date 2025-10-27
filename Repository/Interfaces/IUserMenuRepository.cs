namespace inventory_management_system.Repository.Interfaces
{
    public interface IUserMenuRepository
    {
        Task<List<string>> GetMappedUrlsByRoleIdAsync(int roleId);
    }
}
