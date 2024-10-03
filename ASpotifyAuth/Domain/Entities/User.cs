using System;

namespace ASpotifyAuth.Domain.Entities
{
    public class User: EntityBase
    {
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = "http://localhost:5283/Image/e7224db3-3d67-47e0-8350-c9f4aa3e95dc.webp";
        public string Gender { get;set; } = string.Empty;
        public Guid LovedPlaylistId { get; set; } = Guid.Empty;
        public Guid LatestPlaylistId { get; set; } = Guid.Empty;
        public Guid LatestTrackId { get; set; } = Guid.Empty;
        public List<Guid> Playlists { get; set; } = new List<Guid>();
    }
}
