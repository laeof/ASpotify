namespace ASpotifyAuth.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public Guid LovedPlaylistId { get; set; } = Guid.Empty;
        public Guid LatestPlaylistId { get; set; } = Guid.Empty;
        public Guid LatestTrackId { get; set; } = Guid.Empty;
        public List<Guid> Playlists { get; set; } = new List<Guid>();
    }
}
