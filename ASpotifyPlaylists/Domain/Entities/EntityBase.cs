namespace ASpotifyPlaylists.Domain.Entities
{
    public abstract class EntityBase: IEntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set;} = DateTime.UtcNow;
    }
}
