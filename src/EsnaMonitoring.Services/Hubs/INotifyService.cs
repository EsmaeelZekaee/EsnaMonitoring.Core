namespace EsnaMonitoring.Hubs
{
    using System;
    using System.Threading.Tasks;

    public interface INotifyService
    {
        Task AnErrorOccurredOnEntity(string emthodName, Exception exception, string entity, string type);

        Task EntityAdded(string entity, string type);

        Task EntityRemoved(string entity, string type);

        Task EntityUpdated(string entity, string type);

        Task OnMessage();
    }
}