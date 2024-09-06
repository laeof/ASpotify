using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Domain.Repository.Abstract;

namespace ASpotifyPlaylists.Domain.Repository.Entities
{
    public class PlaylistRepository : ICRUDRepository<Playlist>
    {
        public Task<Playlist> Create()
        {
            throw new NotImplementedException();
        }

        public Task<Playlist> GetById()
        {
            throw new NotImplementedException();
        }

        public Task<Playlist> ModifyById()
        {
            throw new NotImplementedException();
        }

        public Task<Playlist> RemoveById()
        {
            throw new NotImplementedException();
        }
    }
}
