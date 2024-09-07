using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Domain.Repository.Abstract;

namespace ASpotifyPlaylists.Domain
{
    public class DataManager
    {
        public ICRUDRepository<Artist> Artists { get; set; }
        public ICRUDRepository<Playlist> Playlists { get; set; }
        public ICRUDRepository<Track> Tracks { get; set; }
        public DataManager(ICRUDRepository<Artist> Artists,
            ICRUDRepository<Playlist> Playlists,
            ICRUDRepository<Track> Tracks)
        {
            this.Artists = Artists;
            this.Playlists = Playlists;
            this.Tracks = Tracks;
        }
    }
}
