namespace ASpotifyPlaylists.Services.Abstract
{
    public interface ICacheService
    {
        T? GetData<T>(Guid id);
        void SetData<T>(Guid id, T data);
    }
}
