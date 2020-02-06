﻿using EsnaData.Entities;
using EsnaData.Repositories.Interfaces;
using EsnaMonitoring.Hubs;
using EsnaMonitoring.Services.Services.Data.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EsnaMonitoring.Services.Services.Data
{
    public class EntityService<TEntity> : IEntityService<TEntity>
        where TEntity : BaseEntity<long>
    {
        private readonly IHubContext<ModbusHub, INotifyService> _notify;
        private readonly IBaseRepository<TEntity, long> _repository;

        public EntityService(IHubContext<ModbusHub, INotifyService> notify, IBaseRepository<TEntity, long> repository)
        {
            _notify = notify;
            _repository = repository;
        }

        public virtual async ValueTask<TEntity> Add(TEntity entity)
        {
            await _notify.Clients.All.OnMessage();
            try
            {
                await _repository.InsertAsync(entity);
                await _notify.Clients.All.EntityAdded(entity);
            }
            catch (System.Exception e)
            {
                await _notify.Clients.All.AnErrorOccurredOnEntity(nameof(Add), e, entity);
            }
            return entity;
        }

        public virtual async ValueTask<TEntity> Update(TEntity entity)
        {
            try
            {
                await _repository.UpdateAsync(entity);
                await _notify.Clients.All.EntityUpdated(entity);
            }
            catch (System.Exception e)
            {
                await _notify.Clients.All.AnErrorOccurredOnEntity(nameof(Add), e, entity);
            }
            return entity;
        }

        public virtual async Task Removed(TEntity entity)
        {
            try
            {
                await _repository.RemoveAsync(entity);
                await _notify.Clients.All.EntityRemoved(entity);
            }
            catch (System.Exception e)
            {
                await _notify.Clients.All.AnErrorOccurredOnEntity(nameof(Add), e, entity);
            }
        }

        public virtual IAsyncEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return _repository.GetAll().Where(expression).AsAsyncEnumerable();
        }

        public virtual async ValueTask<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression = null)
        {

            return await (expression == null ? _repository.GetAll().Where(expression).FirstAsync() : _repository.GetAll().FirstOrDefaultAsync());
        }


        public virtual async ValueTask<TEntity> Find(long id)
        {

            return await _repository.GetAll().Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async ValueTask UpdateRange(IEnumerable<TEntity> entities)
        {
            await _repository.UpdateRangeAsync(entities);
        }

        public Task SaveChangesAsync()
        {
            return _repository.SaveChangesAsync();
        }
    }
}
