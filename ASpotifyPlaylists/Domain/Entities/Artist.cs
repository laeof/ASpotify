using System.ComponentModel.DataAnnotations.Schema;

namespace ASpotifyPlaylists.Domain.Entities
{
    public class Artist: EntityBase
    {
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Column("Playlists")]
        public List<Guid> Albums { get; set; } = new List<Guid>();
    }
}
