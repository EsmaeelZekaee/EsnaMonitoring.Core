using EsnaData.DbContexts;
using EsnaData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsnaData.Repositories
{
    public class BaseRepository<TEntity, Tkey>
        where TEntity : BaseEntity<Tkey>
    {
        protected EsnaDbContext DbContext { get; set; }
        public BaseRepository(EsnaDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async ValueTask InsertAsync(TEntity entity)
        {
            await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public async ValueTask UpdateAsync(TEntity entity)
        {
            DbContext.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public async ValueTask RemoveAsync(TEntity entity)
        {
            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async ValueTask<TEntity> GetAsync(Tkey id)
        {
            return await DbContext.FindAsync<TEntity>(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>();
        }

    }
}
