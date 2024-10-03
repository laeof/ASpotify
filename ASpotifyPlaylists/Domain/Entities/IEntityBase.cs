namespace ASpotifyPlaylists.Domain.Entities
{
    public interface IEntityBase
    {
        public Guid Id { get; set; }
        public double CreatedDate { get; set; }
        public double UpdatedDate { get; set; }
    }
}
