using ASpotifyPlaylists.Domain.Entities;

namespace ASpotifyPlaylists.Dto
{
    public class PlaylistDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AuthorId { get; set; } = Guid.Empty;
        public double CreatedDate { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public PlaylistTypes Types { get; set; } = 0;
        public List<Track> Tracks { get; set; } = new List<Track>();
    }
}
