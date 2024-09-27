using ASpotifyPlaylists.Domain;
using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Helpers;
using ASpotifyPlaylists.Services.Abstract;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASpotifyPlaylists.Consumers
{
    public class PlaylistConsumer : IMessageConsumer<Playlist>
    {
        private readonly EventingBasicConsumer _eventingBasicConsumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection; 
        public PlaylistConsumer(
            RabbitMQ.Client.IConnectionFactory connectionFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            _connection = connectionFactory.CreateConnection();

            var channel = _connection.CreateModel();

            var queueName = QueueNames.Playlist.ToString();

            channel.QueueDeclare(queue: queueName,
                              durable: true,
                              exclusive: false);

            _eventingBasicConsumer = new EventingBasicConsumer(channel);
            _eventingBasicConsumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var json = JsonConvert.DeserializeObject<MessageDto<object>>(message);

                switch (json!.Action)
                {
                    case Dto.Action.Create:
                        await Create(JsonConvert
                            .DeserializeObject<Playlist>(json.Method.ToString()!)!);
                        break;
                    case Dto.Action.Update:
                        await Update(JsonConvert
                            .DeserializeObject<Playlist>(json.Method.ToString()!)!);
                        break;
                    case Dto.Action.Delete:
                        break;
                    case Dto.Action.AddtrackToPlaylist:
                        await AddTrackToPlaylist(JsonConvert
                            .DeserializeObject<AddTracksToPlaylist>(json.Method.ToString()!)!);
                        break;
                    case Dto.Action.CreateLikedPlaylist:
                        await CreateLikedPlaylist(JsonConvert
                            .DeserializeObject<CreateLikedPlaylist>(json.Method.ToString()!)!);
                        break;
                }
            };

            channel.BasicConsume(queueName, true, _eventingBasicConsumer);
        }

        public async Task<Playlist> Create(Playlist dto)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ASpotifyDbContext>();
                var dataManager = scope.ServiceProvider.GetRequiredService<DataManager>();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                cacheService.SetData(dto.Id, dto);
                await dataManager.Playlists.Create(dto, context.Playlists);
            }

            return dto;
        }

        public async Task<Playlist> Update(Playlist dto)
        {
            Playlist playlist = new Playlist();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ASpotifyDbContext>();
                var dataManager = scope.ServiceProvider.GetRequiredService<DataManager>();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                cacheService.SetData(dto.Id, dto); 
                playlist = await dataManager.Playlists.Modify(dto, context.Playlists);
            }

            return playlist;
        }

        public async Task AddTrackToPlaylist(AddTracksToPlaylist dto)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var playlistService = scope.ServiceProvider.GetRequiredService<IPlaylistService>();
                await playlistService.AddToPlaylist(dto.PlaylistId, dto.TrackId);
            }
        }

        public async Task<Playlist> CreateLikedPlaylist(CreateLikedPlaylist dto)
        {
            var playlist = new Playlist
            {
                AuthorId = dto.UserId,
                ImagePath = "http://localhost:5283/Image/loved.webp",
                Name = "Liked songs",
                Types = PlaylistTypes.Playlist,
                Color = "rgb(61,50,154)"
            };

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var playlistService = scope.ServiceProvider.GetRequiredService<IPlaylistService>();
                var entityMapper = scope.ServiceProvider.GetRequiredService<EntityMapper>();
                var messageProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();

                messageProducer.SendMessage(new MethodAddPlaylistAsLiked(playlist.Id, playlist.AuthorId));
                await playlistService.CreatePlaylist(entityMapper.MapPlaylistDto(playlist));
            }

            return playlist;
        }
    }
}
