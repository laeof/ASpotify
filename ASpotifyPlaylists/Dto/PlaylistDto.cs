namespace ASpotifyPlaylists.Dto
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid AuthorId { get; set; } = Guid.Empty;
        public List<Guid> Tracks { get; set; } = new List<Guid>();
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public PlaylistTypes Types { get; set; } = 0;
    }
}
