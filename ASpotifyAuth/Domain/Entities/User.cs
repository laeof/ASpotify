namespace ASpotifyAuth.Domain.Entities
{
    public class User: BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public Guid LovedPlaylistId { get; set; } = Guid.Empty;
        public Guid LatestPlaylistId { get; set; } = Guid.Empty;
        public Guid LatestTrackId { get; set; } = Guid.Empty;
        public IEnumerable<Guid> PlaylistsId { get; set; } = new List<Guid>();
    }
}
