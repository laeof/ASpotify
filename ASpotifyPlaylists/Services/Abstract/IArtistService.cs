using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;

namespace ASpotifyPlaylists.Services.Abstract
{
    public interface IArtistService
    {
        Task<Artist> CreateArtist(ArtistDto dto);
        Task<Artist> GetArtistById(Guid id);
        Task<Artist> ModifyArtist(ArtistDto dto);
        Task<Artist> DeleteArtist(Guid id);
        Task<Artist> AddPlaylist(Guid artistId, Guid playlistId);
    }
}
