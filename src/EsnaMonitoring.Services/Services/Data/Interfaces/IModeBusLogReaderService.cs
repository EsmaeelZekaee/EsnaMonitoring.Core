namespace EsnaMonitoring.Services.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EsnaMonitoring.Services.Devices;

    public interface IModeBusLogReaderService
    {
        Task<IEnumerable<ModBusDevice>> GetDevicesAsync();
    }
}