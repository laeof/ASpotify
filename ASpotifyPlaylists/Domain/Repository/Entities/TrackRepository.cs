using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Domain.Repository.Abstract;

namespace ASpotifyPlaylists.Domain.Repository.Entities
{
    public class TrackRepository : ICRUDRepository<Track>
    {
        public Task<Track> Create()
        {
            throw new NotImplementedException();
        }

        public Task<Track> GetById()
        {
            throw new NotImplementedException();
        }

        public Task<Track> ModifyById()
        {
            throw new NotImplementedException();
        }

        public Task<Track> RemoveById()
        {
            throw new NotImplementedException();
        }
    }
}
