using ASpotifyAuth.Domain;
using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Services.Abstract;
using System.Security.Claims;

namespace ASpotifyAuth.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ASpotifyDbContext _context;
        private readonly DataManager _dataManager;
        public UserService(IHttpContextAccessor httpContextAccessor,
            ASpotifyDbContext context,
            DataManager dataManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _dataManager = dataManager;
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
    }
}
