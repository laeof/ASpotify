namespace ASpotifyPlaylists.Dto
{
    public class ArtistDto
    {
        public Guid Id { get; set; }
        public double CreatedDate { get; set; } = 0;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<Guid> Albums { get; set; } = new List<Guid>();
    }
}
