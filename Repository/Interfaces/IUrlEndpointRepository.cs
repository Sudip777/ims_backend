using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for URL endpoint repository operations including retrieving, creating, updating, and deleting URL endpoints.
    /// </summary>
    public interface IUrlEndpointRepository
    {
        /// <summary>
        /// Retrieves all URL endpoints from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all URL endpoints</returns>
        Task<IEnumerable<UrlEndpoint>> GetAllUrlEndpointsAsync();
        /// <summary>
        /// Retrieves a URL endpoint by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the URL endpoint to retrieve</param>
        /// <returns>The requested URL endpoint if found, otherwise null</returns>
        Task<UrlEndpoint> GetUrlEndpointByIdAsync(int id);
        /// <summary>
        /// Creates a new URL endpoint in the database asynchronously.
        /// </summary>
        /// <param name="urlEndpoint">The URL endpoint to create</param>
        /// <returns>The created URL endpoint with updated information</returns>
        Task<UrlEndpoint> CreateUrlEndpointAsync(UrlEndpoint urlEndpoint);
        /// <summary>
        /// Updates an existing URL endpoint in the database asynchronously.
        /// </summary>
        /// <param name="urlEndpoint">The URL endpoint data to update</param>
        /// <param name="id">The ID of the URL endpoint to update</param>
        /// <returns>The updated URL endpoint</returns>
        Task<UrlEndpoint> UpdateUrlEndpointAsync(UrlEndpointDto urlEndpoint, int id);
        /// <summary>
        /// Deletes a URL endpoint from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the URL endpoint to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteUrlEndpointAsync(int id);
    }
}
