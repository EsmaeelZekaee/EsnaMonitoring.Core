
namespace EsnaMonitoring.Services.Services.Data
{
    using EsnaData.Entities;
    using EsnaData.Repositories.Interfaces;
    using EsnaMonitoring.Services.Services.Data.Interfaces;
    using EsnaMonitoring.Services.Services.Modbus.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ModBusLogWriterService : IDisposable, IModBusLogWriterService
    {
        private readonly IModbusService _modBusService;
        private readonly IEntityService<Device> _deviceService;
        private readonly IEntityService<Recorde> _recodeService;
        private readonly IEntityService<Configuration> _configurationService;
        private readonly IServiceScope _scope;
        public bool IsConnected { get; private set; }

        public List<Device> ModBusDevices { get; private set; } = new List<Device>();

        public async Task ConnectToDeviceAsync()
        {
            await _modBusService.ConnectAsync();
            IsConnected = true;
        }

        public void Disconnect()
        {
            _modBusService.Disconnect();
            IsConnected = false;
        }


        public async Task ResetDevices()
        {
            await foreach (var item in _deviceService.GetAll(x => x.IsActive == true))
            {
                item.IsActive = false;
            }
            await _deviceService.SaveChangesAsync();
        }
        public async Task GetDevicesAsync()
        {
            await _modBusService.ConnectAsync();
            var devices = _modBusService.GetDevicesAsync();

            await foreach (var item in devices)
            {
                
                var device = await _deviceService.FirstOrDefault(x => x.MacAddress == x.MacAddress) ??
                   new Device()
                   {
                       FirstRegister = item.FirstRegister,
                       Offset = item.Offset,
                       Code = item.Code,
                       CreatedOnUtc = DateTime.UtcNow,
                       MacAddress = item.MacAddress,
                       UnitId = item.UnitId,
                   };
                device.UnitId = item.UnitId;
                device.IsActive = true;
                ModBusDevices.Add(device);
                await _deviceService.Update(device);
            }
        }


        public async Task UpdateAsync()
        {
            
            foreach (var item in ModBusDevices)
            {
                short[] data = await _modBusService.UpdateDeviceAsync(item.UnitId, item.FirstRegister, item.Offset);
                byte[] binary = new byte[data.Length * sizeof(short)];
                Buffer.BlockCopy(data, 0, binary, 0, data.Length);
                var recorde = await _recodeService.Add(new Recorde()
                {
                    DeviceId = item.Id,
                    Data = binary,
                    CreatedOnUtc = DateTime.UtcNow,
                });
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                _modBusService.Dispose();
                _scope.Dispose();
            }
        }

      
        public ModBusLogWriterService(IServiceProvider serviceProvider)
        {
            _scope = serviceProvider.CreateScope();
            _modBusService = _scope.ServiceProvider.GetRequiredService<IModbusService>();
            _deviceService = _scope.ServiceProvider.GetRequiredService<IEntityService<Device>>();
            _recodeService = _scope.ServiceProvider.GetRequiredService<IEntityService<Recorde>>();
            _configurationService = _scope.ServiceProvider.GetRequiredService<IEntityService<Configuration>>();
        }
    }
}
