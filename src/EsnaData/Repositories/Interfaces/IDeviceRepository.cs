namespace EsnaData.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EsnaData.Entities;

    public interface IDeviceRepository : IBaseRepository<Device, long>
    {
        ValueTask<Device> AddOrUpdate(Device device);

        IAsyncEnumerable<Device> GetActiveDevicesAsync();

        ValueTask<Device> GetByMacAddressAsync(string macAddress);

        IAsyncEnumerable<Recorde> GetLatestRecordsAsync();

        ValueTask ResetStatusAsync();
    }
}