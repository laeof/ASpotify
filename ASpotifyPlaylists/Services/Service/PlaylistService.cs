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
        private readonly ITrackService _trackService;
        private readonly ICacheService _cacheService;
        private readonly IMessageProducer _messageProducer;
        
        public PlaylistService(DataManager dataManager,
            ASpotifyDbContext aSpotifyDbContext,
            EntityMapper entityMapper,
            IArtistService artistService,
            ITrackService trackService,
            IMessageProducer messageProducer,
            ICacheService cacheService)
        {
            _dataManager = dataManager;
            _context = aSpotifyDbContext;
            _entityMapper = entityMapper;
            _trackService = trackService;
            _messageProducer = messageProducer;
            _cacheService = cacheService;
        }
        public Task CreatePlaylist(PlaylistDto dto)
        {
            var newentity = _entityMapper.MapDtoPlaylist(dto);

            _messageProducer.SendMessage(
                    new MethodCreate<Playlist>(newentity, QueueNames.Playlist));

            _messageProducer.SendMessage(
                    new MethodAddPlaylistToUser(newentity.Id, dto.AuthorId));

            return Task.CompletedTask;
        }

        public async Task<PlaylistDto> GetPlaylistById(Guid id)
        {
            var playlist = await GetPlaylistWithId(id);

            var playlistDto = await GetFullTrackInfo(playlist);

            return await OrganizeTracksByType(playlist, playlistDto);
        }

        public Task ModifyPlaylist(Playlist dto)
        {
            dto.UpdatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

            _messageProducer.SendMessage(
                   new MethodUpdate<Playlist>(dto, QueueNames.Playlist));

            return Task.CompletedTask;
        }

        public async Task<Playlist> DeletePlaylist(Guid id)
        {
            return await _dataManager.Playlists.RemoveById(id, _context.Playlists);
        }

        public async Task<PlaylistDto> RemoveFromPlaylist(Guid playlistId, List<Guid> listTrackId)
        {
            var playlist = await GetPlaylistWithId(playlistId);
            
            foreach (var track in listTrackId)
            {
                playlist.Tracks.Remove(track);
            }

            //copy list to prevent an exception
            var trackPlaylist = new List<Guid>(playlist.TrackPlaylist);

            foreach (var trackpl in trackPlaylist)
            {
                var trackPlaylistDto = _cacheService.GetData<TrackPlaylist>(trackpl);

                if (trackPlaylistDto == null)
                    trackPlaylistDto = await _dataManager.TrackPlaylists
                        .GetById(trackpl, _context.TrackPlaylists);

                if (listTrackId.Contains(trackPlaylistDto.TrackId))
                {
                    playlist.TrackPlaylist.Remove(trackPlaylistDto.Id);
                    _cacheService.RemoveData(trackpl);
                    await _dataManager.TrackPlaylists.RemoveById(trackPlaylistDto.Id, _context.TrackPlaylists);
                }
            }

            _messageProducer.SendMessage(
                new MethodUpdate<Playlist>(playlist, QueueNames.Playlist));

            var playlistDto = await GetFullTrackInfo(playlist);

            return await OrganizeTracksByType(playlist, playlistDto);
        }

        public async Task<PlaylistDto> AddToPlaylist(Guid playlistId, List<Guid> listTrackId)
        {
            var playlist = await GetPlaylistWithId(playlistId);

            foreach (var track in listTrackId)
                playlist.Tracks.Add(track);

            if (playlist.Types != PlaylistTypes.Playlist)
            {
                _messageProducer.SendMessage(
                    new MethodUpdate<Playlist>(playlist, QueueNames.Playlist));

                return _entityMapper.MapPlaylistDto(playlist);
            }

            foreach(var track in listTrackId)
            {
                var trackPlaylist = new TrackPlaylist
                {
                    TrackId = track,
                    PlaylistId = playlistId,
                    CreatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds
                };

                _cacheService.SetData(trackPlaylist.Id, trackPlaylist);
                playlist.TrackPlaylist.Add(trackPlaylist.Id);
                await _dataManager.TrackPlaylists.Create(trackPlaylist, _context.TrackPlaylists);
            }

            _messageProducer.SendMessage(
                    new MethodUpdate<Playlist>(playlist, QueueNames.Playlist));
            
            var playlistDto = await GetFullTrackInfo(playlist);

            return await OrganizeTracksByType(playlist, playlistDto);
        }

        public async Task<List<PlaylistDto>> GetPopularPlaylists()
        {
            var playlists = await _dataManager.PlaylistsRepository.GetPopular();

            var playlistDto = new List<PlaylistDto>();

            foreach (var playlist in playlists)
                playlistDto.Add(_entityMapper.MapPlaylistDto(playlist));

            return playlistDto;
        }
    
        private async Task<Playlist> GetPlaylistWithId(Guid playlistId)
        {
            var playlist = _cacheService.GetData<Playlist>(playlistId);

            if (playlist == null)
            {
                playlist = await _dataManager.Playlists.GetById(playlistId, _context.Playlists);
                _cacheService.SetData(playlistId, playlist);
            }

            return playlist;
        }

        private async Task<PlaylistDto> GetFullTrackInfo(Playlist playlist)
        {
            var playlistDto = _entityMapper.MapPlaylistDto(playlist);

            foreach (var track in playlist.Tracks)
            {
                var trackDto = await _trackService.GetTrackById(track);
                playlistDto.Tracks.Add(trackDto);
            }

            return playlistDto;
        }

        private async Task<PlaylistDto> OrganizeTracksByType(Playlist playlist, PlaylistDto playlistDto)
        {
            if (playlist.Types != PlaylistTypes.Playlist)
            {
                playlistDto.Tracks.Sort((x, y) => x.CreatedDate.CompareTo(y.CreatedDate));
                return playlistDto;
            }

            var trackPlaylists = new List<TrackPlaylist>();

            foreach (var trackpl in playlist.TrackPlaylist)
            {
                var trackPlaylistDto = _cacheService.GetData<TrackPlaylist>(trackpl);

                if (trackPlaylistDto == null)
                {
                    trackPlaylistDto = await _dataManager.TrackPlaylists
                        .GetById(trackpl, _context.TrackPlaylists);
                    _cacheService.SetData(trackpl, trackPlaylistDto);
                }

                var track = playlistDto.Tracks.FirstOrDefault(x => x.Id == trackPlaylistDto.TrackId);

                if (track != null)
                    track.CreatedDate = trackPlaylistDto.CreatedDate;
            }

            playlistDto.Tracks = playlistDto.Tracks.OrderByDescending(x => x.CreatedDate)!.ToList();

            return playlistDto;
        }
    }
}
