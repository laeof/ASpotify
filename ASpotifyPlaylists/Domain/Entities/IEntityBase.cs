namespace ASpotifyPlaylists.Domain.Entities
{
    public interface IEntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
