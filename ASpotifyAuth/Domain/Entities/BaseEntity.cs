namespace ASpotifyAuth.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id => Guid.NewGuid();
        public DateTime CreateDateTime => DateTime.UtcNow;
    }
}
