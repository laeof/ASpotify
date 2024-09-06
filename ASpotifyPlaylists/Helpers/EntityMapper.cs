using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;

namespace ASpotifyPlaylists.Helpers
{
    public class EntityMapper
    {
        public PlaylistDto MapPlaylistDto(Playlist playlist)
        {
            var dto = new PlaylistDto
            {
                Id = playlist.Id,
                CreatedDate = playlist.CreatedDate,
                AuthorId = playlist.AuthorId,
                ImagePath = playlist.ImagePath,
                Name = playlist.Name,
                Tracks = playlist.Tracks,
                Types = playlist.Types
            };

            return dto;
        }
    }
}
