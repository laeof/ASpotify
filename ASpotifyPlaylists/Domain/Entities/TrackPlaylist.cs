namespace ASpotifyPlaylists.Domain.Entities
{
    public class TrackPlaylist: EntityBase
    {
        public Guid TrackId { get; set; } = Guid.Empty;
        public Guid PlaylistId { get; set; } = Guid.Empty;
    }
}
