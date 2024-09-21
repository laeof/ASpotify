using ASpotifyPlaylists.Domain.Entities;

namespace ASpotifyPlaylists.Dto
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }
        public double CreatedDate { get; set; } = 0;
        public Guid AuthorId { get; set; } = Guid.Empty;
        public List<Track> Tracks { get; set; } = new List<Track>();
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public PlaylistTypes Types { get; set; } = 0;
        public string Color { get; set; } = string.Empty;
    }
}
