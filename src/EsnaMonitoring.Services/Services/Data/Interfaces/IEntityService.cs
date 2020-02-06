using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EsnaMonitoring.Services.Services.Data.Interfaces
{
    public interface IEntityService<TEntity>
    {
        ValueTask<TEntity> Add(TEntity entity);

        Task Removed(TEntity entity);
        
        ValueTask<TEntity> Find(long id);
        
        ValueTask<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression = null);
        
        IAsyncEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        
        ValueTask<TEntity> Update(TEntity entity);
        
        ValueTask UpdateRange(IEnumerable<TEntity> entities);
        Task SaveChangesAsync();
    }
}