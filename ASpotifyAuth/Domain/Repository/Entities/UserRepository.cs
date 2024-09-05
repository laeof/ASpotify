using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Domain.Repository.Abstract;

namespace ASpotifyAuth.Domain.Repository.Entities
{
    public class UserRepository : ICRUDRepository<User>
    {
        public Task<User> Create()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById()
        {
            throw new NotImplementedException();
        }

        public Task<User> ModifyById()
        {
            throw new NotImplementedException();
        }

        public Task<User> RemoveById()
        {
            throw new NotImplementedException();
        }
    }
}
