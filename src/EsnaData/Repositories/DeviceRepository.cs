namespace EsnaData.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EsnaData.DbContexts;
    using EsnaData.Entities;
    using EsnaData.Repositories.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class DeviceRepository : BaseRepository<Device, long>, IDeviceRepository
    {
        public DeviceRepository(EsnaDbContext dbContext)
            : base(dbContext)
        {
        }

        public async ValueTask<Device> AddOrUpdate(Device device)
        {
            var originalDevice =
                await this.DbContext.Devices.FirstOrDefaultAsync(x => x.MacAddress == device.MacAddress) ?? device;
            originalDevice.UnitId = device.UnitId;
            originalDevice.IsActive = device.IsActive;
            if (originalDevice.Id == default) this.DbContext.Devices.Add(originalDevice);
            else this.DbContext.Devices.Update(originalDevice);

            await this.DbContext.SaveChangesAsync();

            return originalDevice;
        }

        public IAsyncEnumerable<Device> GetActiveDevicesAsync()
        {
            return this.DbContext.Devices.Where(x => x.IsActive).Select(
                x => new Device
                         {
                             Id = x.Id,
                             IsActive = x.IsActive,
                             UnitId = x.UnitId,
                             Code = x.Code,
                             CreatedOnUtc = x.CreatedOnUtc,
                             MacAddress = x.MacAddress,
                             ExteraInfornamtion = x.ExteraInfornamtion
                         }).AsAsyncEnumerable();
        }

        public async ValueTask<Device> GetByMacAddressAsync(string macAddress)
        {
            return await this.DbContext.Devices.FirstOrDefaultAsync(x => x.MacAddress == macAddress);
        }

        public IAsyncEnumerable<Recorde> GetLatestRecordsAsync()
        {
            return this.DbContext.Recordes.Include(x => x.Device).Where(x => x.Device.IsActive).Select(
                x => new Recorde
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
            await this.DbContext.Database.ExecuteSqlRawAsync(
                $"UPDATE {nameof(Device)} SET {nameof(Device.IsActive)} = 0 WHERE {nameof(Device.IsActive)} = @p0",
                0);
        }
    }
}