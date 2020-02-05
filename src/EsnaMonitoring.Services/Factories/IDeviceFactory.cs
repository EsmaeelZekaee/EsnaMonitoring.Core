using EsnaMonitoring.Services.Devices;

namespace EsnaMonitoring.Services.Factories
{
    public interface IDeviceFactory
    {
        ModBusDevice CreateDevice(byte address, string codeAndSerial);
    }
}