using ASpotifyAuth.Domain.Entities;

namespace ASpotifyAuth.Services.Abstract
{
    public interface IUserService
    {
        Task<User> GetUser();
        Task<User> GetUserById(Guid id);
        Task<string> GetUsername();
    }
}
