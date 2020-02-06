using EsnaData.Entities;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace EsnaData.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        IQueryable<TEntity> GetAll();

        ValueTask<TEntity> GetAsync(Tkey id);
        
        ValueTask InsertAsync(TEntity entity);
        
        ValueTask RemoveAsync(TEntity entity);
        
        ValueTask UpdateAsync(TEntity entity);

        ValueTask UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task SaveChangesAsync();
    }
}