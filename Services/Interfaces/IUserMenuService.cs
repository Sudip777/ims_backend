using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
   
        public interface IUserMenuService
        {
            Task<List<NavItemDto>> GetMenuItemsAsync(int roleId);
        }
    
}
