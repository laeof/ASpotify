namespace ASpotifyAuth.Domain.Entities
{
    public abstract class EntityBase : IEntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public double CreatedDate { get; set; } = 0;
        public double UpdatedDate { get; set; } = 0;
    }
}
