using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Domain.Repository.Abstract;

namespace ASpotifyAuth.Domain
{
    public class DataManager
    {
        public ICRUDRepository<User> Users { get; set; }
        public DataManager(ICRUDRepository<User> Users)
        {
            this.Users = Users;
        }
    }
}
