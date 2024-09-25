using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;

namespace ASpotifyPlaylists.Services.Abstract
{
    public interface IPlaylistService
    {
        Task<Playlist> CreatePlaylist(PlaylistDto dto);
        Task<PlaylistDto> GetPlaylistById(Guid id);
        Task<List<PlaylistDto>> GetPopularPlaylists();
        Task<Playlist> ModifyPlaylist(Playlist dto);
        Task<Playlist> DeletePlaylist(Guid id);
        Task<Playlist> AddToPlaylist(Guid playlistId, Guid trackId);
    }
}
