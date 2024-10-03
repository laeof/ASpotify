using ASpotifyAuth.Domain;
using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Dto;
using ASpotifyAuth.Services.Abstract;
using System.Security.Claims;

namespace ASpotifyAuth.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ASpotifyDbContext _context;
        private readonly DataManager _dataManager;
        private readonly MessageProducer _messageProducer;
        public UserService(IHttpContextAccessor httpContextAccessor,
            ASpotifyDbContext context,
            DataManager dataManager,
            MessageProducer messageProducer)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _dataManager = dataManager;
            _messageProducer = messageProducer;
        }

        public async Task<string> GetUsername()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
                throw new Exception("UserServiceException:NoUser");

            var userid = user.FindFirstValue(ClaimTypes.Name);

            if(userid == null)
                throw new Exception("UserServiceException:NoUserId");

            return (await _dataManager.Users.GetById(Guid.Parse(userid), _context.Users)).UserName;
        }
        public async Task<User> GetUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
                throw new Exception("UserServiceException:NoUser");

            var userid = user.FindFirstValue(ClaimTypes.Name);

            if (userid == null)
                throw new Exception("UserServiceException:NoUserId");

            return await _dataManager.Users.GetById(Guid.Parse(userid), _context.Users);
        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = await _dataManager.Users.GetById(id, _context.Users);

            if (user == null)
                throw new Exception("UserServiceException:NoUser");

            if(user.AvatarUrl.StartsWith("http://localhost:5283"))
                user.AvatarUrl = user.AvatarUrl.Replace("http://localhost:5283", "http://hope1ess.local:5283");

            _messageProducer.SendMessage(
                    new MethodUpdate<User>(user, QueueNames.User));    

            return user;
        }
    }
}
