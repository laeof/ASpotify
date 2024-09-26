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

            if (playlist.Types != PlaylistTypes.Playlist)
            {
                playlistDto.Tracks.Sort((x, y) => x.CreatedDate.CompareTo(y.CreatedDate));
                return playlistDto;
            }

            var trackPlaylists = new List<TrackPlaylist>();

            foreach (var trackpl in playlist.TrackPlaylist)
            {
                var trackPlaylistDto = await _dataManager.TrackPlaylists
                    .GetById(trackpl, _context.TrackPlaylists);

                trackPlaylists.Add(trackPlaylistDto);
            }

            foreach (var trackpl in trackPlaylists)
            {
                var track = playlistDto.Tracks.FirstOrDefault(x => x.Id == trackpl.TrackId);

                if(track != null)
                    track.CreatedDate = trackpl.CreatedDate;
            }

            playlistDto.Tracks = playlistDto.Tracks.OrderByDescending(x => x.CreatedDate)!.ToList();

            return playlistDto;
        }

        public async Task<Playlist> ModifyPlaylist(Playlist dto)
        {
            dto.UpdatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

            return await _dataManager.Playlists.Modify(dto, _context.Playlists);
        }

        public async Task<Playlist> DeletePlaylist(Guid id)
        {
            return await _dataManager.Playlists.RemoveById(id, _context.Playlists);
        }

        public async Task<Playlist> AddToPlaylist(Guid playlistId, Guid trackId)
        {
            var playlist = await _dataManager.Playlists.GetById(playlistId, _context.Playlists);

            playlist.Tracks.Add(trackId);

            if (playlist.Types != PlaylistTypes.Playlist)
            {
                await ModifyPlaylist(playlist);

                return playlist;
            }

            var trackPlaylist = new TrackPlaylist
            {
                TrackId = trackId,
                PlaylistId = playlistId,
                CreatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds
            };

            await _dataManager.TrackPlaylists.Create(trackPlaylist, _context.TrackPlaylists);
            
            playlist.TrackPlaylist.Add(trackPlaylist.Id);

            await ModifyPlaylist(playlist);

            return playlist;
        }

        public async Task<List<PlaylistDto>> GetPopularPlaylists()
        {
            var playlists = await _dataManager.PlaylistsRepository.GetPopular();

            var playlistDto = new List<PlaylistDto>();

            foreach (var playlist in playlists)
                playlistDto.Add(_entityMapper.MapPlaylistDto(playlist));

            return playlistDto;
        }
    }
}
