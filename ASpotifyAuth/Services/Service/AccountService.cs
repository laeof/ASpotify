using ASpotifyAuth.Domain;
using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Dto;
using ASpotifyAuth.Services.Abstract;
using System.Text.RegularExpressions;

namespace ASpotifyAuth.Services.Service
{
    public class AccountService: IAccountService
    {
        private readonly DataManager _dataManager;
        private readonly ASpotifyDbContext _context;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IMessageProducer _messageProducer;
        public AccountService(DataManager dataManager,
            ASpotifyDbContext context,
            IPasswordHasherService passwordHasher,
            IMessageProducer messageProducer) 
        {
            _dataManager = dataManager;
            _context = context;
            _passwordHasher = passwordHasher;
            _messageProducer = messageProducer;
        }

        public User Register(RegisterDto dto)
        {
            var newentity = new User
            {
                Email = dto.Email,
                UserName = dto.Username,
                Gender = dto.Gender,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                CreatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds,
            };

            _messageProducer.SendMessage(
                   new MethodCreate<User>(newentity, QueueNames.User));

            return newentity;
        }

        //true - exist
        //false - not exist
        public async Task<bool> CheckExistsEmail(string email)
        {
            return await _dataManager.UsersRepository.ValidEmail(email) != null;
        }
        public bool CheckValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (Regex.IsMatch(email, emailPattern))
                return true;

            return false;
        }
        public async Task<bool> CheckValidUsername(string username)
        {
            return await _dataManager.UsersRepository.ValidUsername(username);
        }

        public async Task<bool> CheckValidPassword(LoginDto dto)
        {
            var user = await _dataManager.UsersRepository.ValidEmail(dto.Email);

            if (user == null)
                return false;

            return _passwordHasher.Verify(dto.Password, user.PasswordHash);
        }

        public async Task<User> GetUserAsync(LoginDto dto)
        {
            return (await _dataManager.UsersRepository.ValidEmail(dto.Email))!;
        }

        public async Task<UserRefreshToken> AddUserRefreshTokens(UserRefreshToken user)
        {
            user.CreatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            return await _dataManager.RefreshTokens.Create(user, _context.RefreshTokens);
        }

        public async Task<UserRefreshToken> GetSavedRefreshTokens(Guid id, string refreshtoken)
        {
            var tokens = await _dataManager.RefreshTokensRepository
                .GetRefreshTokensByUserId(id);

            UserRefreshToken? token = null;

            foreach (var refreshToken in tokens)
            {
                if(refreshToken.RefreshToken == refreshtoken)
                {
                    token = refreshToken;
                    break;
                }
            }

            if(token == null) 
                throw new Exception("Refresh token does not exists");

            if (token.IsExpired)
                throw new Exception("Token is expired");

            if (token.RefreshToken != refreshtoken)
                throw new Exception("Refresh token is not equal to actual");

            return token;
        }

        public async Task<UserRefreshToken> DeleteUserRefreshTokens(Guid id, string refreshToken)
        {
            return await _dataManager.RefreshTokens.RemoveById(id, _context.RefreshTokens);
        }
    }
}
