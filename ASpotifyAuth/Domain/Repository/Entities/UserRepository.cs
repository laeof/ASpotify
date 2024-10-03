using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Domain.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ASpotifyAuth.Domain.Repository.Entities
{
    public class UserRepository: IUserRepository
    {
        private readonly ASpotifyDbContext _context;
        public UserRepository(ASpotifyDbContext context)
        {
            _context = context;
        }
        //true - not valid
        //false - valid
        public async Task<bool> ValidUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username) != null;
        }
        public async Task<User?> ValidEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
