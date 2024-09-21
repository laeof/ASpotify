using ASpotifyPlaylists.Domain;
using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Helpers;
using ASpotifyPlaylists.Services.Abstract;

namespace ASpotifyPlaylists.Services.Service
{
    public class PlaylistService : IPlaylistService
    {
        private readonly DataManager _dataManager;
        private readonly ASpotifyDbContext _context;
        private readonly EntityMapper _entityMapper;
        private readonly IArtistService _artistService; 
        private readonly ITrackService _trackService;
        private readonly static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        public PlaylistService(DataManager dataManager,
            ASpotifyDbContext aSpotifyDbContext,
            EntityMapper entityMapper,
            IArtistService artistService,
            ITrackService trackService)
        {
            _dataManager = dataManager;
            _context = aSpotifyDbContext;
            _entityMapper = entityMapper;
            _artistService = artistService;
            _trackService = trackService;
        }
        public async Task<Playlist> CreatePlaylist(PlaylistDto dto)
        {
            var newentity = _entityMapper.MapDtoPlaylist(dto);

            await _semaphoreSlim.WaitAsync();
            try
            {
                await _dataManager.Playlists.Create(newentity, _context.Playlists);
                await _artistService.AddPlaylist(dto.AuthorId, newentity.Id);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
            return newentity;
        }

        public async Task<PlaylistDto> GetPlaylistById(Guid id)
        {
            var playlist = await _dataManager.Playlists.GetById(id, _context.Playlists);

            var playlistDto = _entityMapper.MapPlaylistDto(playlist);

            foreach (var track in playlist.Tracks)
            {
                var trackDto = await _trackService.GetTrackById(track);
                playlistDto.Tracks.Add(trackDto);
            }

            playlistDto.Tracks.Sort((x, y) => x.CreatedDate.CompareTo(y.CreatedDate));

            return playlistDto;
        }

        public async Task<Playlist> ModifyPlaylist(Playlist dto)
        {
            var entity = new Playlist();

            entity.Id = dto.Id;
            entity.Name = dto.Name;
            entity.AuthorId = dto.AuthorId;
            entity.ImagePath = dto.ImagePath;
            entity.Tracks = dto.Tracks;
            entity.UpdatedDate = dto.CreatedDate;
            entity.CreatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            entity.Types = dto.Types;
            entity.Color = dto.Color;

            return await _dataManager.Playlists.Modify(entity, _context.Playlists);
        }

        public async Task<Playlist> DeletePlaylist(Guid id)
        {
            return await _dataManager.Playlists.RemoveById(id, _context.Playlists);
        }

        public async Task<Playlist> AddToPlaylist(Guid playlistId, Guid trackId)
        {
            var playlistDto = await GetPlaylistById(playlistId);

            var playlist = _entityMapper.MapDtoPlaylist(playlistDto);

            playlist.Tracks.Add(trackId);

            await ModifyPlaylist(playlist);

            return playlist;
        }
    }
}
