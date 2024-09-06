using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Domain.Repository.Abstract;

namespace ASpotifyPlaylists.Domain.Repository.Entities
{
    public class ArtistRepository : ICRUDRepository<Artist>
    {
        public Task<Artist> Create()
        {
            throw new NotImplementedException();
        }

        public Task<Artist> GetById()
        {
            throw new NotImplementedException();
        }

        public Task<Artist> ModifyById()
        {
            throw new NotImplementedException();
        }

        public Task<Artist> RemoveById()
        {
            throw new NotImplementedException();
        }
    }
}
