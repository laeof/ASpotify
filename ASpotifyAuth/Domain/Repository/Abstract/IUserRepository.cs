using ASpotifyAuth.Domain.Entities;

namespace ASpotifyAuth.Domain.Repository.Abstract
{
    public interface IUserRepository
    {
        Task<bool> ValidUsername(string username);
        Task<User?> ValidEmail(string email);
    }
}
