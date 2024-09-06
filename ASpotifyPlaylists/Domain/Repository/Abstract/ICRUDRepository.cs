namespace ASpotifyPlaylists.Domain.Repository.Abstract
{
    public interface ICRUDRepository<T>
    {
        public Task<T> GetById();
        public Task<T> Create();
        public Task<T> ModifyById();
        public Task<T> RemoveById();
    }
}
