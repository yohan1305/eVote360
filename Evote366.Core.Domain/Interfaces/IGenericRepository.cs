using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Evote366.Core.Domain.Interfaces
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity?> AddAsync(TEntity entity);
        Task<List<TEntity>?> AddRangeAsync(List<TEntity> entities);
        Task DeleteAsync(int id);
        Task<List<TEntity>> GetAllList();
        IQueryable<TEntity> GetAllQuery();
        Task<TEntity?> GetById(int id);
        Task<TEntity?> UpdateAsync(int id, TEntity entity);
        Task<List<TEntity>> GetAllListWithInclude(List<string> properties);
        IQueryable<TEntity> GetAllQueryWithInclude(List<string> properties);

    }
}
