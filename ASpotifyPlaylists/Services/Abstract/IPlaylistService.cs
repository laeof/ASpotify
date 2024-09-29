using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;

namespace ASpotifyPlaylists.Services.Abstract
{
    public interface IPlaylistService
    {
        Task CreatePlaylist(PlaylistDto dto);
        Task<PlaylistDto> GetPlaylistById(Guid id);
        Task<List<PlaylistDto>> GetPopularPlaylists();
        Task ModifyPlaylist(Playlist dto);
        Task<Playlist> DeletePlaylist(Guid id);
        Task<PlaylistDto> AddToPlaylist(Guid playlistId, List<Guid> listTrackId);
        Task<PlaylistDto> RemoveFromPlaylist(Guid playlistId, List<Guid> listTrackId);

    }
}
