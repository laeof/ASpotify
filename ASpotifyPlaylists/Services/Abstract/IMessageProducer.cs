using ASpotifyPlaylists.Dto;

namespace ASpotifyPlaylists.Services.Abstract
{
    public interface IMessageProducer
    {
        public void SendMessage<T>(MessageDto<T> message);
    }
}
