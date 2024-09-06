using ASpotifyPlaylists.Domain;
using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Services.Abstract;

namespace ASpotifyPlaylists.Services.Service
{
    public class ArtistService : IArtistService
    {
        private readonly DataManager _dataManager;
        private readonly ASpotifyDbContext _context;
        public ArtistService(DataManager dataManager,
            ASpotifyDbContext aSpotifyDbContext)
        {
            _dataManager = dataManager;
            _context = aSpotifyDbContext;
        }
        public async Task<Artist> CreateArtist(ArtistDto dto)
        {
            var newentity = new Artist
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Albums = dto.Albums,
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
            entity.UpdatedDate = entity.CreatedDate;
            entity.CreatedDate = dto.CreatedDate;
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
    }
}
