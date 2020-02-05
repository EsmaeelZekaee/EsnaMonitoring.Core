using EsnaData.Entities;
using EsnaData.Repositories;
using EsnaMonitoring.Services.Devices;
using EsnaMonitoring.Services.Services.Modbus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EsnaMonitoring.Services.Services.Databse
{
    public class RecordeUpdaterService //: IDisposable
    {
        private readonly IModbusService _modBusService;
        private readonly DeviceRepository _deviceRepository;
        private readonly RecordeRepository _recordeRepository;
        private readonly ConfigorationRepository _configorationRepository;
        private readonly IServiceScope _scope;
        private bool _isConected = false;
        public List<Device> ModBusDevices { get; private set; } = new List<Device>();

        public async Task Update()
        {
            if (_isConected == false)
            {
                await _deviceRepository.ResetStatusAsync();
                await _modBusService.ConnectAsync();
                _isConected = true;
                var devoces = _modBusService.GetDevicesAsync();
                await foreach (var item in devoces)
                {
                    var device = await _deviceRepository.AddOrUpdate(new Device()
                    {
                        IsActive = true,
                        FirstRegister = item.FirstRegister,
                        Offset = item.Offset,
                        Code = item.Code,
                        CreatedOnUtc = DateTime.UtcNow,
                        MacAddress = item.MacAddress,
                        UnitId = item.UnitId,
                    });
                    ModBusDevices.Add(device);
                }
            }
            foreach (var item in ModBusDevices)
            {
                short[] data = await _modBusService.UpdateDeviceAsync(item.UnitId, item.FirstRegister, item.Offset);
                byte[] binary = new byte[data.Length * sizeof(short)];
                Buffer.BlockCopy(data, 0, binary, 0, data.Length);
                await _recordeRepository.InsertAsync(new Recorde()
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

        public RecordeUpdaterService(IServiceProvider serviceProvider)
        {
            _scope = serviceProvider.CreateScope();
            _modBusService = _scope.ServiceProvider.GetRequiredService<IModbusService>();
            _deviceRepository = _scope.ServiceProvider.GetRequiredService<DeviceRepository>();
            _recordeRepository = _scope.ServiceProvider.GetRequiredService<RecordeRepository>();
            _configorationRepository = _scope.ServiceProvider.GetRequiredService<ConfigorationRepository>();
        }
    }
}
