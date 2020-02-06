using EsnaData.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnaData.Repositories.Interfaces
{
    public interface IDeviceRepository : IBaseRepository<Device, long>
    {
        ValueTask<Device> AddOrUpdate(Device device);
        IAsyncEnumerable<Device> GetActiveDevicesAsync();
        ValueTask<Device> GetByMacAddressAsync(string macAddress);
        IAsyncEnumerable<Recorde> GetLatestRecordsAsync();
        ValueTask ResetStatusAsync();
    }
}