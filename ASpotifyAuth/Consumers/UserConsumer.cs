using ASpotifyAuth.Domain.Entities;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using ASpotifyAuth.Dto;
using System.Text;
using Newtonsoft.Json;
using ASpotifyAuth.Domain;
using ASpotifyAuth.Services.Abstract;

namespace ASpotifyAuth.Consumers
{
    public class UserConsumer : IMessageConsumer<User>
    {
        private readonly EventingBasicConsumer _eventingBasicConsumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        public UserConsumer(
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

            var queueName = QueueNames.User.ToString();

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
                            .DeserializeObject<User>(json.Method.ToString()!)!);
                        break;
                    case Dto.Action.Update:
                        await Update(JsonConvert
                            .DeserializeObject<User>(json.Method.ToString()!)!);
                        break;
                    case Dto.Action.Delete:
                        break;
                    case Dto.Action.AddPlaylistAsLiked:
                        await AddPlaylistAsLiked(JsonConvert
                            .DeserializeObject<AddPlaylistToUser>(json.Method.ToString()!)!);
                        break;
                }
            };

            channel.BasicConsume(queueName, true, _eventingBasicConsumer);
        }
        public async Task<User> Create(User dto)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dataManager = scope.ServiceProvider.GetRequiredService<DataManager>();
                var context = scope.ServiceProvider.GetRequiredService<ASpotifyDbContext>();
                var messageProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();

                messageProducer.SendMessage(new MethodCreateLikedPlaylist(dto.Id));
                await dataManager.Users.Create(dto, context.Users);
            }

            return dto;
        }

        public async Task<User> Update(User dto)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dataManager = scope.ServiceProvider.GetRequiredService<DataManager>();
                var context = scope.ServiceProvider.GetRequiredService<ASpotifyDbContext>();

                await dataManager.Users.Modify(dto, context.Users);
            }

            return dto;
        }
        
        public async Task<User> AddPlaylistAsLiked(AddPlaylistToUser dto)
        {
            User user = new User();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dataManager = scope.ServiceProvider.GetRequiredService<DataManager>();
                var context = scope.ServiceProvider.GetRequiredService<ASpotifyDbContext>();
                var messageProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();

                user = await dataManager.Users.GetById(dto.ArtistId, context.Users);
                user.LovedPlaylistId = dto.PlaylistId;

                messageProducer.SendMessage(new MethodUpdate<User>(user, QueueNames.User));
            }
            return user;
        }
    }
}
