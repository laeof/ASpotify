using ASpotifyPlaylists.Domain;
using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Services.Abstract;

namespace ASpotifyPlaylists.Services.Service
{
    public class TrackService : ITrackService
    {
        private readonly DataManager _dataManager;
        private readonly ASpotifyDbContext _context;
        public TrackService( 
            DataManager dataManager,
            ASpotifyDbContext aSpotifyDbContext)
        {
            _dataManager = dataManager;
            _context = aSpotifyDbContext;
        }
        public async Task<Track> CreateTrack(TrackDto dto)
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
                CreatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds,
                UpdatedDate = dto.CreatedDate

            };
            
            await _dataManager.Tracks.Create(track, _context.Tracks);

            return track;
        }

        public async Task<Track> GetTrackById(Guid id)
        {
            return await _dataManager.Tracks.GetById(id, _context.Tracks);
        }

        public async Task<Track> ModifyTrack(TrackDto dto)
        {
            var track = new Track();

            track.Id = dto.Id;
            track.Name = dto.Name;
            //track.AlbumId = dto.AlbumId;
            track.ArtistId = dto.ArtistId;
            track.Duration = dto.Duration;
            track.ImagePath = dto.ImagePath;
            track.UrlPath = dto.UrlPath;
            track.UpdatedDate = dto.CreatedDate;
            track.CreatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

            return await _dataManager.Tracks.Modify(track, _context.Tracks);
        }

        public async Task<Track> DeleteTrack(Guid id)
        {
            return await _dataManager.Tracks.RemoveById(id, _context.Tracks);
        }
    }
}
