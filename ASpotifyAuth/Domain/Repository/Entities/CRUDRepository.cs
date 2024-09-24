using ASpotifyAuth.Domain.Entities;
using ASpotifyAuth.Domain.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ASpotifyAuth.Domain.Repository.Entities
{
    public class CRUDRepository<T> : ICRUDRepository<T> where T : class, IEntityBase, new()
    {
        private readonly ASpotifyDbContext _context;
        public CRUDRepository(ASpotifyDbContext aSpotifyDbContext) 
        {
            _context = aSpotifyDbContext;
        }
        public async Task<T> Create(T Dto, DbSet<T> dbSet) 
        {
            _context.Entry(Dto).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return Dto;
        }

        public async Task<T> GetById(Guid id, DbSet<T> dbSet)
        {
            var Dto = await dbSet.FirstOrDefaultAsync(dto => dto.Id == id);

            if (Dto == null)
                return new T();
            
            return Dto;
        }

        public async Task<T> Modify(T Dto, DbSet<T> dbSet)
        {
            var dto = await dbSet.FirstOrDefaultAsync(dto => dto.Id == Dto.Id);

            if (dto == null)
                return new T();

            _context.Entry(dto).CurrentValues.SetValues(Dto);

            _context.Entry(dto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<T> RemoveById(Guid id, DbSet<T> dbSet)
        {
            var Dto = await dbSet.FirstOrDefaultAsync(dto => dto.Id == id);

            if (Dto == null)
                return new T();

            _context.Remove(Dto);
            await _context.SaveChangesAsync();

            return Dto;
        }
    }
}
