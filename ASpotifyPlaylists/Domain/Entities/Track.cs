namespace ASpotifyPlaylists.Domain.Entities
{
    public class Track : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string UrlPath {  get; set; } = string.Empty;
        public Guid ArtistId { get; set; } = Guid.Empty;
        public Guid AlbumId { get; set; } = Guid.Empty;
        public int Duration { get; set; } = 0;
    }
}
