using ASpotifyAuth.Domain.Entities;

namespace ASpotifyAuth.Domain.Repository.Abstract
{
    public interface IRefreshTokenRepository
    {
        Task<List<UserRefreshToken>> GetRefreshTokensByUserId(Guid userId);
    }
}
