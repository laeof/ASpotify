using ASpotifyAuth.Dto;

namespace ASpotifyAuth.Services.Abstract
{
    public interface IMessageProducer
    {
        public void SendMessage<T>(MessageDto<T> message);
    }
}
