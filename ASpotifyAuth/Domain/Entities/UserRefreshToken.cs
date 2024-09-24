namespace ASpotifyAuth.Domain.Entities
{
    public class UserRefreshToken: EntityBase
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public bool IsExpired { get; set; } = false;
    }
}
