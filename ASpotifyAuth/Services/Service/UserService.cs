using ASpotifyAuth.Services.Abstract;

namespace ASpotifyAuth.Services.Service
{
    public class UserService : IUserService
    {
        public string GetMyName()
        {
            return "name";
        }
    }
}
