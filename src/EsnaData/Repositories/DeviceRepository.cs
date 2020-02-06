using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsnaData.DbContexts;
using EsnaData.Entities;
using Microsoft.EntityFrameworkCore;
using EsnaData.Repositories.Interfaces;

namespace EsnaData.Repositories
{
    public class DeviceRepository : BaseRepository<Device, long>, IDeviceRepository
    {
        public DeviceRepository(EsnaDbContext dbContext) : base(dbContext)
        {
        }

        public async ValueTask<Device> GetByMacAddressAsync(string macAddress)
        {
            return await DbContext.Devices.FirstOrDefaultAsync(x => x.MacAddress == macAddress);
        }

        public async ValueTask<Device> AddOrUpdate(Device device)
        {
            var originalDevice = await DbContext.Devices.FirstOrDefaultAsync(x => x.MacAddress == device.MacAddress) ?? device;
            originalDevice.UnitId = device.UnitId;
            originalDevice.IsActive = device.IsActive;
            if (originalDevice.Id == default)
                DbContext.Devices.Add(originalDevice);
            else
                DbContext.Devices.Update(originalDevice);

            await DbContext.SaveChangesAsync();

            return originalDevice;

        }

        public IAsyncEnumerable<Device> GetActiveDevicesAsync()
        {
            return DbContext.Devices.Where(x => x.IsActive).Select(x => new Device
            {
                Id = x.Id,
                IsActive = x.IsActive,
                UnitId = x.UnitId,
                Code = x.Code,
                CreatedOnUtc = x.CreatedOnUtc,
                MacAddress = x.MacAddress,
                ExteraInfornamtion = x.ExteraInfornamtion,
            }).AsAsyncEnumerable();
        }

        public IAsyncEnumerable<Recorde> GetLatestRecordsAsync()
        {
            return DbContext.Recordes.Include(x => x.Device).Where(x => x.Device.IsActive).Select(x => new Recorde
            {
                Id = x.Id,
                CreatedOnUtc = x.CreatedOnUtc,
                DeviceId = x.DeviceId,
                Data = x.Data,
                Device = x.Device
            }).AsAsyncEnumerable();
        }

        public async ValueTask ResetStatusAsync()
        {
            await DbContext.Database.ExecuteSqlRawAsync($"UPDATE {nameof(Device)} SET {nameof(Device.IsActive)} = 0 WHERE {nameof(Device.IsActive)} = @p0", 0);

        }
    }
}
