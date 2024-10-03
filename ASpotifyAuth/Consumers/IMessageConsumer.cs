namespace ASpotifyAuth.Consumers
{
    public interface IMessageConsumer<T>
    {
        Task<T> Create(T dto);
        Task<T> Update(T dto);
    }
}
