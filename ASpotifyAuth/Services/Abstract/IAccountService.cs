using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Dto;

namespace ASpotifyAuth.Services.Abstract
{
    public interface IAccountService
    {
        bool CheckValidEmail(string email);
        Task<bool> CheckExistsEmail(string email);
        Task<bool> CheckValidUsername(string username);
        Task<bool> CheckValidPassword(LoginDto dto);
        User Register(RegisterDto dto);
        Task<User> GetUserAsync(LoginDto dto);
        Task<UserRefreshToken> AddUserRefreshTokens(UserRefreshToken user);
        Task<UserRefreshToken> GetSavedRefreshTokens(Guid id, string refreshtoken);
        Task<UserRefreshToken> DeleteUserRefreshTokens(Guid id, string refreshToken);
    }
}
