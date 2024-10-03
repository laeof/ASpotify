using ASpotifyPlaylists.Domain.Entities;

namespace ASpotifyPlaylists.Domain.Repository.Abstract
{
    public interface IPlaylistRepository
    {
        Task<List<Playlist>> GetPopular();
    }
}
