namespace EsnaMonitoring.Services.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EsnaData.Entities;

    public interface IModBusLogWriterService
    {
        bool IsConnected { get; }

        List<Device> ModBusDevices { get; }

        Task ConnectToDeviceAsync();

        void Disconnect();

        Task GetDevicesAsync();

        Task ResetDevices();

        Task UpdateAsync();
    }
}