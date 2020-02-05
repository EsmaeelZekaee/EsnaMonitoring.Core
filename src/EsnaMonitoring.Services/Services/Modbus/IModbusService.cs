using EsnaMonitoring.Services.Devices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnaMonitoring.Services.Services.Modbus
{
    public interface IModbusService : IDisposable
    {
        void Disconnect();

        Task ConnectAsync();

        IAsyncEnumerable<ModBusDevice> GetDevicesAsync();

        ValueTask<short[]> UpdateDeviceAsync(ModBusDevice device);

        ValueTask<short[]> UpdateDeviceAsync(byte unitId, byte firstRegister, byte offset);
    }
}