using EsnaData.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EsnaMonitoring.Services.Services.Data.Interfaces
{
    public interface IModBusLogWriterService
    {
        List<Device> ModBusDevices { get; }
        bool IsConnected { get; }

        Task ConnectToDeviceAsync();
        void Disconnect();
        Task GetDevicesAsync();
        Task UpdateAsync();
        Task ResetDevices();
    }
}