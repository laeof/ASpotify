using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using ASpotifyPlaylists.Services.Abstract;

namespace ASpotifyPlaylists.Consumers
{
    public class ArtistConsumer : IMessageConsumer<Artist>
    {
        private readonly EventingBasicConsumer _eventingBasicConsumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        public ArtistConsumer(
            RabbitMQ.Client.IConnectionFactory connectionFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            while (true)
            {
                try
                {
                    _connection = connectionFactory.CreateConnection();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка подключения к RabbitMQ: {ex.Message}. Повторная попытка через 5 секунд...");
                    Thread.Sleep(5000);
                }
            }
            
            var channel = _connection.CreateModel();

            var queueName = QueueNames.Artist.ToString();

            channel.QueueDeclare(queue: queueName,
                              durable: true,
                              exclusive: false);

            _eventingBasicConsumer = new EventingBasicConsumer(channel);
            _eventingBasicConsumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var json = JsonConvert.DeserializeObject<MessageDto<object>>(message);

                switch(json!.Action)
                {
                    case Dto.Action.Create:
                        break;
                    case Dto.Action.Update:
                        break;
                    case Dto.Action.Delete:
                        break;
                    case Dto.Action.AddplaylistToUser:
                        await AddPlaylistToUser(JsonConvert
                            .DeserializeObject<AddPlaylistToUser>(json.Method.ToString()!)!);
                        break;
                }
            };

            channel.BasicConsume(queueName, true, _eventingBasicConsumer);
        }

        public async Task<Artist> Create(Artist dto)
        {
            return new Artist();
        }

        public async Task<Artist> Update(Artist dto)
        {
            return new Artist();
        }

        public async Task AddPlaylistToUser(AddPlaylistToUser dto)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var artistService = scope.ServiceProvider.GetRequiredService<IArtistService>();
                await artistService.AddPlaylist(dto.ArtistId, dto.PlaylistId);
            }

            return;
        }
    }
}
