using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly Evote365DbContext _context;

        public GenericRepository(Evote365DbContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity?> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<List<TEntity>?> AddRangeAsync(List<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task<TEntity?> UpdateAsync(int id, TEntity entity)
        {
            var entry = await _context.Set<TEntity>().FindAsync(id);

            if (entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entry;
            }

            return null;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<List<TEntity>> GetAllList()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAllListWithInclude(List<string> properties)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> GetById(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual IQueryable<TEntity> GetAllQuery()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public virtual IQueryable<TEntity> GetAllQueryWithInclude(List<string> properties)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return query;
        }

    }
}
