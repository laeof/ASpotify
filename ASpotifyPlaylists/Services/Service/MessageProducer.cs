using ASpotifyPlaylists.Consumers;
using ASpotifyPlaylists.Dto;
using ASpotifyPlaylists.Services.Abstract;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using static ASpotifyPlaylists.Services.Service.MessageProducer;

namespace ASpotifyPlaylists.Services.Service
{
    public class MessageProducer : IMessageProducer
    {
        private readonly RabbitMQ.Client.IConnectionFactory _connection;
        public MessageProducer(
            RabbitMQ.Client.IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory;
        }
        public void SendMessage<T>(MessageDto<T> message)
        {
            using var connection = _connection.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(
                              queue: message.QueueNames.ToString(),
                              durable: true,
                              exclusive: false);

            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish("", message.QueueNames.ToString(), body: body);
        }
    }
}
