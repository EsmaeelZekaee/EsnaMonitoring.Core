namespace EsnaMonitoring.Services.Services.Modbus.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EsnaMonitoring.Services.Devices;

    public interface IModbusService : IDisposable
    {
        Task ConnectAsync();

        void Disconnect();

        IAsyncEnumerable<ModBusDevice> GetDevicesAsync();

        ValueTask<short[]> UpdateDeviceAsync(ModBusDevice device);

        ValueTask<short[]> UpdateDeviceAsync(byte unitId, byte firstRegister, byte offset);
    }
}