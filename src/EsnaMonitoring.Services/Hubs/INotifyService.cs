using EsnaData.Entities;
using System;
using System.Threading.Tasks;

namespace EsnaMonitoring.Hubs
{
    public interface INotifyService
    {
        Task OnMessage();
        Task EntityAdded(string entity, string type);
        Task EntityUpdated(string entity, string type);
        Task EntityRemoved(string entity, string type);
        Task AnErrorOccurredOnEntity(string emthodName, Exception exception, string entity, string type);
    }
}
