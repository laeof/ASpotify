using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Domain.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ASpotifyAuth.Domain.Repository.Entities
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ASpotifyDbContext _context;
        public RefreshTokenRepository(ASpotifyDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserRefreshToken>> GetRefreshTokensByUserId(Guid userId)
        {
            return await _context.RefreshTokens.Where(token => token.UserId == userId).ToListAsync();
        }
    }
}
