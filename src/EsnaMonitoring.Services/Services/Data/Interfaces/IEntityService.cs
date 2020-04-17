namespace EsnaMonitoring.Services.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IEntityService<TEntity>
    {
        ValueTask<TEntity> AddAsync(TEntity entity);

        ValueTask<TEntity> FindAsync(long id);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression = null);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression = null);

        IAsyncEnumerable<TEntity> GetAllAsync(Expression<Func<TEntity, bool>> expression = null);

        Task RemovedAsync(TEntity entity);

        Task SaveChangesAsync();

        ValueTask<TEntity> UpdateAsync(TEntity entity);

        ValueTask UpdateRangeAsync(IEnumerable<TEntity> entities);
    }
}