namespace ASpotifyPlaylists.Dto
{
    public class TrackDto
    {
        public Guid Id { get; set; }
        public Guid ArtistId { get; set; } = Guid.Empty;
        public Guid AlbumId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string UrlPath { get; set; } = string.Empty;
        public double CreatedDate { get; set; } = 0;
        public int Duration { get; set; } = 0;
    }
}
