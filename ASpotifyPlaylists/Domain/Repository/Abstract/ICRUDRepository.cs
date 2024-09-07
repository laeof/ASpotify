using ASpotifyPlaylists.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASpotifyPlaylists.Domain.Repository.Abstract
{
    public interface ICRUDRepository<T> where T : class, IEntityBase, new()
    {
        Task<T> GetById(Guid id, DbSet<T> dbSet);
        Task<T> Create(T dto, DbSet<T> dbSet);
        Task<T> Modify(T dto, DbSet<T> dbSet);
        Task<T> RemoveById(Guid id, DbSet<T> dbSet);
    }
}
