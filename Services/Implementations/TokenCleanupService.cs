using Microsoft.EntityFrameworkCore;
using inventory_management_system.Data;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class TokenCleanupService : BackgroundService, ITokenCleanupService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TokenCleanupService> _logger;

        public TokenCleanupService(IServiceScopeFactory scopeFactory, ILogger<TokenCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        /// <summary>
        /// This method runs automatically as a background task.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Token cleanup background service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.UtcNow;
                    var nextMidnight = now.Date.AddDays(1);
                    var delay = nextMidnight - now;

                    // Wait until midnight
                    await Task.Delay(delay, stoppingToken);
                    await CleanupExpiredTokensAsync();
                }
               
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during token cleanup background process.");
                }
            }
        }

        /// <summary>
        /// Deletes all expired refresh tokens.
        /// This can be called manually or by the background job.
        /// </summary>
        public async Task CleanupExpiredTokensAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

            var expiredTokens = await context.Tokens
                .Where(t => t.ExpiresAt < DateTime.UtcNow)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                context.Tokens.RemoveRange(expiredTokens);
                await context.SaveChangesAsync();
                _logger.LogInformation($"Deleted {expiredTokens.Count} expired tokens at {DateTime.UtcNow}.");
            }
            else
            {
                _logger.LogInformation("No expired tokens found for cleanup.");
            }
        }
    }
}
