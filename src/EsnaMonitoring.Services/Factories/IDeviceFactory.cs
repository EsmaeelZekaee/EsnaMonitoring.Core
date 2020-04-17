namespace EsnaMonitoring.Services.Factories
{
    using EsnaMonitoring.Services.Devices;

    public interface IDeviceFactory
    {
        ModBusDevice CreateDevice(byte address, string codeAndSerial);
    }
}