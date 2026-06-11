namespace Payroll.Data
{
    using Microsoft.EntityFrameworkCore;
    using Payroll.Domain;

    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly PayrollDbContext _dbContext;

        public RefreshTokenRepository(PayrollDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds a new refresh token to the database.
        /// This method is used when a user logs in or refreshes their access token, and a new refresh token needs to be generated and stored securely.
        /// </summary>
        /// <param name="token">Refresh token to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns a task representing the asynchronous operation</returns>
        public async Task AddRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken)
        {
            _dbContext.RefreshTokens.Add(token);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates an existing refresh token in the database.
        /// </summary>
        /// <param name="token">Refresh token to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns a task representing the asynchronous operation</returns>
        public async Task UpdateRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken)
        {
            _dbContext.RefreshTokens.Update(token);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a refresh token by its hash value.
        /// </summary>
        /// <param name="tokenHash">Hash of the refresh token</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns the matching refresh token or null if not found</returns>
        public async Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash, CancellationToken cancellationToken)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash
                                                                           && !x.IsRevoked
                                                                           && x.ExpiryDate > DateTime.UtcNow, cancellationToken);
        }

        /// <summary>
        /// Revokes a refresh token by marking it as revoked and setting the revoked timestamp.
        /// </summary>
        /// <param name="token">Refresh token to revoke</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns a task representing the asynchronous operation</returns>
        public async Task<bool> RevokeRefreshTokenAsync(RefreshToken token, CancellationToken cancellationToken)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
