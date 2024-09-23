using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Domain.Repository.Abstract;

namespace ASpotifyAuth.Domain
{
    public class DataManager
    {
        public ICRUDRepository<User> Users { get; set; }
        public ICRUDRepository<UserRefreshToken> RefreshTokens { get; set; }
        public IUserRepository UsersRepository {  get; set; }
        public IRefreshTokenRepository RefreshTokensRepository { get; set; }
        public DataManager(
            ICRUDRepository<User> users,
            IUserRepository usersRepository,
            ICRUDRepository<UserRefreshToken> refreshTokens,
            IRefreshTokenRepository refreshTokensRepository)
        {
            Users = users;
            RefreshTokens = refreshTokens;
            UsersRepository = usersRepository;
            RefreshTokensRepository = refreshTokensRepository;
        }
    }
}
