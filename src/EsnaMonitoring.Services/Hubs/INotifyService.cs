using EsnaData.Entities;
using System;
using System.Threading.Tasks;

namespace EsnaMonitoring.Hubs
{
    public interface INotifyService
    {
        Task OnMessage();
        Task EntityAdded(BaseEntity<long> entity);
        Task EntityUpdated(BaseEntity<long> entity);
        Task EntityRemoved(BaseEntity<long> entity);
        Task AnErrorOccurredOnEntity<TEntity>(string emthodName, Exception exception, TEntity entity) where TEntity : BaseEntity<long>;
    }
}
