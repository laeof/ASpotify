using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;

namespace ASpotifyPlaylists.Services.Abstract
{
    public interface IPlaylistService
    {
        Task<Playlist> CreatePlaylist(PlaylistDto dto);
        Task<Playlist> GetPlaylistById(Guid id);
        Task<Playlist> ModifyPlaylist(PlaylistDto dto);
        Task<Playlist> DeletePlaylist(Guid id);
        Task<Playlist> AddToPlaylist(Guid playlistId, Guid trackId);
    }
}
