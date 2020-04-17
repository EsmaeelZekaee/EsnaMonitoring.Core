namespace EsnaData.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EsnaData.Entities;

    public interface IBaseRepository<TEntity, Tkey>
        where TEntity : BaseEntity<Tkey>
    {
        IQueryable<TEntity> GetAll();

        ValueTask<TEntity> GetAsync(Tkey id);

        ValueTask InsertAsync(TEntity entity);

        ValueTask RemoveAsync(TEntity entity);

        Task SaveChangesAsync();

        ValueTask UpdateAsync(TEntity entity);

        ValueTask UpdateRangeAsync(IEnumerable<TEntity> entities);
    }
}