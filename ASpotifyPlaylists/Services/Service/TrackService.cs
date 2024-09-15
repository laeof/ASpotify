using ASpotifyPlaylists.Domain;
using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Helpers;
using ASpotifyPlaylists.Services.Abstract;
using System.Threading;

namespace ASpotifyPlaylists.Services.Service
{
    public class TrackService : ITrackService
    {
        private readonly IPlaylistService _playlistService;
        private readonly DataManager _dataManager;
        private readonly ASpotifyDbContext _context;
        private readonly static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        public TrackService(IPlaylistService playlistService, 
            DataManager dataManager,
            ASpotifyDbContext aSpotifyDbContext)
        {
            _playlistService = playlistService;
            _dataManager = dataManager;
            _context = aSpotifyDbContext;
        }
        public async Task<Track> CreateTrack(TrackDto dto)
        {
            var track = new Track
            {
                Name = dto.Name,
                AlbumId = dto.AlbumId,
                ArtistId = dto.ArtistId,
                Duration = dto.Duration,
                ImagePath = dto.ImagePath,
                UrlPath = dto.UrlPath,
            };
            await _semaphoreSlim.WaitAsync();
            try
            {
                await _dataManager.Tracks.Create(track, _context.Tracks);
                await _playlistService.AddToPlaylist(dto.AlbumId, track.Id);
            }
            finally
            {
                _semaphoreSlim.Release();
            }

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
            track.UpdatedDate = track.CreatedDate;
            track.CreatedDate = dto.CreatedDate;

            return await _dataManager.Tracks.Modify(track, _context.Tracks);
        }

        public async Task<Track> DeleteTrack(Guid id)
        {
            return await _dataManager.Tracks.RemoveById(id, _context.Tracks);
        }
    }
}
