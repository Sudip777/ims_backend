using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class UrlEndpointRepository : IUrlEndpointRepository
    {
        private readonly ApplicationDBContext _context;
        public UrlEndpointRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UrlEndpoint>> GetAllUrlEndpointsAsync()
        {
            return await _context.UrlEndpoints.ToListAsync();
        }

        public async Task<UrlEndpoint> GetUrlEndpointByIdAsync(int id)
        {
            return await _context.UrlEndpoints
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UrlEndpointId == id);
        }
        public async Task<UrlEndpoint> CreateUrlEndpointAsync(UrlEndpoint urlEndpoint)
        {
             _context.UrlEndpoints.Add(urlEndpoint);
             await  _context.SaveChangesAsync();
             return urlEndpoint;
        }

        public async Task<UrlEndpoint> UpdateUrlEndpointAsync(UrlEndpointDto urlEndpoint, int id)
        {
            var existingUrlEndpoint = await _context.UrlEndpoints
                .FirstOrDefaultAsync(p => p.UrlEndpointId == id);

            if (existingUrlEndpoint == null)
                throw new Exception("Customer not found");

            existingUrlEndpoint.Url = urlEndpoint.Url;
            existingUrlEndpoint.Description = urlEndpoint.Description;
         

            await _context.SaveChangesAsync();

            return existingUrlEndpoint;
        }

        public async Task<bool> DeleteUrlEndpointAsync(int id)
        {
            var urlEndpoint = await _context.UrlEndpoints.FindAsync(id);

            if (urlEndpoint == null)
                throw new KeyNotFoundException($"Url Endpoint with ID {id} not found.");
            _context.UrlEndpoints.Remove(urlEndpoint);

            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

    }
}
