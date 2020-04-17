namespace EsnaMonitoring.Services.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using EsnaData.Entities;
    using EsnaData.Repositories.Interfaces;

    using EsnaMonitoring.Hubs;
    using EsnaMonitoring.Services.Services.Data.Interfaces;

    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    public class EntityService<TEntity> : IEntityService<TEntity>
        where TEntity : BaseEntity<long>
    {
        private readonly IHubContext<ModbusHub, INotifyService> _notify;

        private readonly IBaseRepository<TEntity, long> _repository;

        public EntityService(IHubContext<ModbusHub, INotifyService> notify, IBaseRepository<TEntity, long> repository)
        {
            this._notify = notify;
            this._repository = repository;
        }

        public virtual async ValueTask<TEntity> AddAsync(TEntity entity)
        {
            await this._notify.Clients.All.OnMessage();
            try
            {
                await this._repository.InsertAsync(entity);
                await this._notify.Clients.All.EntityAdded(entity.AsJson(), typeof(TEntity).AssemblyQualifiedName);
            }
            catch (Exception e)
            {
                await this._notify.Clients.All.AnErrorOccurredOnEntity(
                    nameof(this.AddAsync),
                    e,
                    entity.AsJson(),
                    typeof(TEntity).AssemblyQualifiedName);
            }

            return entity;
        }

        public virtual async ValueTask<TEntity> FindAsync(long id)
        {
            return await this._repository.GetAll().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            var q = this._repository.GetAll();

            if (expression != null) q = q.Where(expression);
            return q.FirstOrDefaultAsync();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression = null)
        {
            var q = this._repository.GetAll();

            if (expression != null) q = q.Where(expression);
            return q.FirstOrDefault();
        }

        public virtual IAsyncEnumerable<TEntity> GetAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return (expression == null ? this._repository.GetAll() : this._repository.GetAll().Where(expression))
                .AsAsyncEnumerable();
        }

        public virtual async Task RemovedAsync(TEntity entity)
        {
            try
            {
                await this._repository.RemoveAsync(entity);
                await this._notify.Clients.All.EntityRemoved(entity.AsJson(), typeof(TEntity).AssemblyQualifiedName);
            }
            catch (Exception e)
            {
                await this._notify.Clients.All.AnErrorOccurredOnEntity(
                    nameof(this.AddAsync),
                    e,
                    entity.AsJson(),
                    typeof(TEntity).AssemblyQualifiedName);
            }
        }

        public Task SaveChangesAsync()
        {
            return this._repository.SaveChangesAsync();
        }

        public virtual async ValueTask<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                await this._repository.UpdateAsync(entity);
                await this._notify.Clients.All.EntityUpdated(entity.AsJson(), typeof(TEntity).AssemblyQualifiedName);
            }
            catch (Exception e)
            {
                await this._notify.Clients.All.AnErrorOccurredOnEntity(
                    nameof(this.AddAsync),
                    e,
                    entity.AsJson(),
                    typeof(TEntity).AssemblyQualifiedName);
            }

            return entity;
        }

        public async ValueTask UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            await this._repository.UpdateRangeAsync(entities);
        }
    }
}