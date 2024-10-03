using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Dto;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using ASpotifyPlaylists.Services.Abstract;
using ASpotifyPlaylists.Domain;

namespace ASpotifyPlaylists.Consumers
{
    public class TrackConsumer : IMessageConsumer<Track>
    {
        private readonly EventingBasicConsumer _eventingBasicConsumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        public TrackConsumer(
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

            var queueName = QueueNames.Track.ToString();

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
                            .DeserializeObject<Track>(json.Method.ToString()!)!);
                        break;
                    case Dto.Action.Update:
                        await Update(JsonConvert
                            .DeserializeObject<Track>(json.Method.ToString()!)!);
                        break;
                    case Dto.Action.Delete:
                        break;
                }
            };

            channel.BasicConsume(queueName, true, _eventingBasicConsumer);
        }

        public async Task<Track> Create(Track dto)
        {
            Track track = new Track();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ASpotifyDbContext>();
                var dataManager = scope.ServiceProvider.GetRequiredService<DataManager>();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                cacheService.SetData(dto.Id, dto);
                track = await dataManager.Tracks.Create(dto, context.Tracks);
            }

            return track;
        }

        public async Task<Track> Update(Track dto)
        {
            Track track = new Track();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ASpotifyDbContext>();
                var dataManager = scope.ServiceProvider.GetRequiredService<DataManager>();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                cacheService.SetData(dto.Id, dto);
                track = await dataManager.Tracks.Modify(dto, context.Tracks);
            }

            return track;
        }
    }
}
