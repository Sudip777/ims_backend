namespace inventory_management_system.Services.Interfaces
{
   
        public interface ITokenCleanupService
        {
            /// <summary>
            /// Deletes all expired refresh tokens from the database.
            /// </summary>
            Task CleanupExpiredTokensAsync();
        }
    
}
