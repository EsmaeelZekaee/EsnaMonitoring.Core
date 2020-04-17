namespace EsnaMonitoring.Services.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EsnaData.Entities;

    using EsnaMonitoring.Services.Services.Data.Interfaces;
    using EsnaMonitoring.Services.Services.Modbus.Interfaces;

    using Microsoft.Extensions.DependencyInjection;

    public class ModBusLogWriterService : IDisposable, IModBusLogWriterService
    {
        private readonly IEntityService<Configuration> _configurationService;

        private readonly IEntityService<Device> _deviceService;

        private readonly IModbusService _modBusService;

        private readonly IEntityService<Recorde> _recodeService;

        private readonly IServiceScope _scope;

        public ModBusLogWriterService(IServiceProvider serviceProvider)
        {
            this._scope = serviceProvider.CreateScope();
            this._modBusService = this._scope.ServiceProvider.GetRequiredService<IModbusService>();
            this._deviceService = this._scope.ServiceProvider.GetRequiredService<IEntityService<Device>>();
            this._recodeService = this._scope.ServiceProvider.GetRequiredService<IEntityService<Recorde>>();
            this._configurationService =
                this._scope.ServiceProvider.GetRequiredService<IEntityService<Configuration>>();
        }

        public bool IsConnected { get; private set; }

        public List<Device> ModBusDevices { get; } = new List<Device>();

        public async Task ConnectToDeviceAsync()
        {
            await this._modBusService.ConnectAsync();
            this.IsConnected = true;
        }

        public void Disconnect()
        {
            this._modBusService.Disconnect();
            this.IsConnected = false;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public async Task GetDevicesAsync()
        {
            await this._modBusService.ConnectAsync();
            var devices = this._modBusService.GetDevicesAsync();

            await foreach (var item in devices)
            {
                var macAddress = item.MacAddress;
                var device = await this._deviceService.FirstOrDefaultAsync(x => x.MacAddress == macAddress).ContinueWith(
                                 x =>
                                     {
                                         if (x.IsFaulted)
                                         {
                                         }

                                         return x;
                                     }).GetAwaiter().GetResult();
                device = device ?? new Device
                                       {
                                           FirstRegister = item.FirstRegister,
                                           Offset = item.Offset,
                                           Code = item.Code,
                                           CreatedOnUtc = DateTime.UtcNow,
                                           MacAddress = item.MacAddress,
                                           UnitId = item.UnitId
                                       };
                device.UnitId = item.UnitId;
                device.IsActive = true;
                this.ModBusDevices.Add(device);
                await this._deviceService.UpdateAsync(device);
            }
        }

        public async Task ResetDevices()
        {
            await foreach (var item in this._deviceService.GetAllAsync(x => x.IsActive)) item.IsActive = false;
            await this._deviceService.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            foreach (var item in this.ModBusDevices)
            {
                short[] data = await this._modBusService.UpdateDeviceAsync(
                                   item.UnitId,
                                   item.FirstRegister,
                                   item.Offset);
                byte[] binary = new byte[data.Length * sizeof(short)];
                Buffer.BlockCopy(data, 0, binary, 0, data.Length);
                var recorde = await this._recodeService.AddAsync(
                                  new Recorde { DeviceId = item.Id, Data = binary, CreatedOnUtc = DateTime.UtcNow });
            }
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                this._modBusService.Dispose();
                this._scope.Dispose();
            }
        }
    }
}