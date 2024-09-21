using ASpotifyPlaylists.Domain;
using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Helpers;
using ASpotifyPlaylists.Services.Abstract;

namespace ASpotifyPlaylists.Services.Service
{
    public class ArtistService : IArtistService
    {
        private readonly DataManager _dataManager;
        private readonly ASpotifyDbContext _context;
        private readonly EntityMapper _entityMapper;
        public ArtistService(DataManager dataManager,
            ASpotifyDbContext aSpotifyDbContext,
            EntityMapper entityMapper)
        {
            _dataManager = dataManager;
            _context = aSpotifyDbContext;
            _entityMapper = entityMapper;
        }
        public async Task<Artist> CreateArtist(ArtistDto dto)
        {
            var newentity = new Artist
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Albums = dto.Albums,
                CreatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds,
                UpdatedDate = dto.CreatedDate

            };

            var entity = await _dataManager.Artists.Create(newentity, _context.Artists);

            return entity;
        }

        public async Task<Artist> GetArtistById(Guid id)
        {
            return await _dataManager.Artists.GetById(id, _context.Artists);
        }

        public async Task<Artist> ModifyArtist(ArtistDto dto)
        {
            var entity = new Artist();

            entity.Id = dto.Id;
            entity.UpdatedDate = dto.CreatedDate;
            entity.CreatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            entity.UserName = dto.UserName;
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Albums = dto.Albums;

            return await _dataManager.Artists.Modify(entity, _context.Artists);
        }
        public async Task<Artist> DeleteArtist(Guid id)
        {
            return await _dataManager.Artists.RemoveById(id, _context.Artists);
        }

        public async Task<Artist> AddPlaylist(Guid artistId, Guid playlistId)
        {
            var artist = await GetArtistById(artistId);

            artist.Albums.Add(playlistId);

            await ModifyArtist(_entityMapper.MapArtistDto(artist));

            return artist;
        }
    }
}
