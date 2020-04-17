namespace EsnaData.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EsnaData.DbContexts;
    using EsnaData.Entities;
    using EsnaData.Repositories.Interfaces;

    public class BaseRepository<TEntity, Tkey> : IBaseRepository<TEntity, Tkey>
        where TEntity : BaseEntity<Tkey>
    {
        public BaseRepository(EsnaDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        protected EsnaDbContext DbContext { get; set; }

        public IQueryable<TEntity> GetAll()
        {
            return this.DbContext.Set<TEntity>();
        }

        public async ValueTask<TEntity> GetAsync(Tkey id)
        {
            return await this.DbContext.FindAsync<TEntity>(id);
        }

        public async ValueTask InsertAsync(TEntity entity)
        {
            await this.DbContext.AddAsync(entity);
            await this.DbContext.SaveChangesAsync();
        }

        public async ValueTask RemoveAsync(TEntity entity)
        {
            this.DbContext.Remove(entity);
            await this.DbContext.SaveChangesAsync();
        }

        public Task SaveChangesAsync()
        {
            return this.DbContext.SaveChangesAsync();
        }

        public async ValueTask UpdateAsync(TEntity entity)
        {
            this.DbContext.Update(entity);
            await this.DbContext.SaveChangesAsync();
        }

        public async ValueTask UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            this.DbContext.UpdateRange(entities);
            await this.DbContext.SaveChangesAsync();
        }
    }
}