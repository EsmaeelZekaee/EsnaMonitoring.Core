using EsnaMonitoring.Services.Devices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnaMonitoring.Services.Services.Data.Interfaces
{
    public interface IModeBusLogReaderService
    {
        Task<IEnumerable<ModBusDevice>> GetDevicesAsync();
    }
}