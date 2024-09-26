using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;

namespace ASpotifyPlaylists.Helpers
{
    public class EntityMapper
    {
        public Playlist MapDtoPlaylist(PlaylistDto dto)
        {
            var entity = new Playlist();
            entity.Id = dto.Id;

            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();

            entity.Name = dto.Name;
            entity.AuthorId = dto.AuthorId;
            entity.ImagePath = dto.ImagePath;
            entity.Tracks = dto.Tracks.Select(track => track.Id).ToList();
            entity.UpdatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            entity.CreatedDate = dto.CreatedDate;
            entity.Types = dto.Types;
            entity.Color = dto.Color;

            return entity;
        }
        public PlaylistDto MapPlaylistDto(Playlist playlist)
        {
            var dto = new PlaylistDto
            {
                Id = playlist.Id,
                CreatedDate = playlist.CreatedDate,
                AuthorId = playlist.AuthorId,
                ImagePath = playlist.ImagePath,
                Name = playlist.Name,
                Types = playlist.Types,
                Tracks = new List<Track>(),
                Color = playlist.Color,
            };

            //NO TRACKS INPUT

            return dto;
        }

        public ArtistDto MapArtistDto(Artist artist)
        {
            var dto = new ArtistDto
            {
                Id = artist.Id,
                Albums = artist.Albums,
                CreatedDate = artist.CreatedDate,
                FirstName = artist.FirstName,
                LastName = artist.LastName,
                UserName = artist.UserName
            };

            return dto;
        }

        public Track MapDtoTrack(TrackDto dto)
        {
            var track = new Track
            {
                Id = dto.Id,
                Name = dto.Name,
                AlbumId = dto.AlbumId,
                ArtistId = dto.ArtistId,
                Duration = dto.Duration,
                ImagePath = dto.ImagePath,
                UrlPath = dto.UrlPath,
                CreatedDate = dto.CreatedDate,
                UpdatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds
            };

            return track;
        }
    }
}
