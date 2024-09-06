using ASpotifyPlaylists.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASpotifyPlaylists.Domain.Entities
{
    public class Playlist: EntityBase
    {
        public Guid AuthorId { get; set; } = Guid.Empty;
        [ForeignKey("TrackId")]
        public List<Guid> Tracks { get; set; } = new List<Guid>();
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public PlaylistTypes Types { get; set; } = 0;
    }
}
