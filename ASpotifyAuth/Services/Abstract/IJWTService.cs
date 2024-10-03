using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Dto;
using System.Security.Claims;

namespace ASpotifyAuth.Services.Abstract
{
    public interface IJWTService
    {
        Tokens GenerateToken(Guid id);
        Tokens GenerateRefreshToken(Guid id);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool ExpireAccessToken(string token);
    }
}
