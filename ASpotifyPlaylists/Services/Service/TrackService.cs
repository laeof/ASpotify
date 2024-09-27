using ASpotifyPlaylists.Domain;
using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Helpers;
using ASpotifyPlaylists.Services.Abstract;

namespace ASpotifyPlaylists.Services.Service
{
    public class TrackService : ITrackService
    {
        private readonly DataManager _dataManager;
        private readonly ASpotifyDbContext _context;
        private readonly EntityMapper _entityMapper;
        private readonly IMessageProducer _messageProducer;
        private readonly ICacheService _cacheService;
        public TrackService( 
            DataManager dataManager,
            ASpotifyDbContext aSpotifyDbContext,
            EntityMapper entityMapper,
            IMessageProducer messageProducer,
            ICacheService cacheService)
        {
            _dataManager = dataManager;
            _context = aSpotifyDbContext;
            _entityMapper = entityMapper;
            _messageProducer = messageProducer;
            _cacheService = cacheService;
        }
        public Task CreateTrack(List<TrackDto> dto)
        {
            foreach (var track in dto)
            {
                var newentity = _entityMapper.MapDtoTrack(track);
                newentity.CreatedDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

                _messageProducer.SendMessage(
                        new MethodCreate<Track>(newentity, QueueNames.Track));
            }

            _messageProducer.SendMessage(
                    new MethodAddTracksToPlaylist(
                        dto.Select(x => x.Id).ToList(), 
                        dto[0].AlbumId));

            return Task.CompletedTask;
        }

        public async Task<Track> GetTrackById(Guid id)
        {
            var entity = _cacheService.GetData<Track>(id);

            if (entity == null) {
                entity = await _dataManager.Tracks.GetById(id, _context.Tracks);
                _cacheService.SetData(id, entity);
            }

            return entity;
        }

        public Task ModifyTrack(TrackDto dto)
        {
            var newentity = _entityMapper.MapDtoTrack(dto);

            _messageProducer.SendMessage(
                   new MethodUpdate<Track>(newentity, QueueNames.Track));

            return Task.CompletedTask;
        }

        public async Task<Track> DeleteTrack(Guid id)
        {
            return await _dataManager.Tracks.RemoveById(id, _context.Tracks);
        }
    }
}
