namespace ASpotifyPlaylists.Dto
{
    public class TrackDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid ArtistId { get; set; } = Guid.Empty;
        public Guid AlbumId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string UrlPath { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int Duration { get; set; } = 0;
    }
}
