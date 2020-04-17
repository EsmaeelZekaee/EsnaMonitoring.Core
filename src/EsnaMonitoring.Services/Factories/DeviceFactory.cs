#nullable enable
namespace EsnaMonitoring.Services.Factories
{
    using System.Text.RegularExpressions;

    using EsnaMonitoring.Services.Devices;
    using EsnaMonitoring.Services.Exceptions;

    public class DeviceFactory : IDeviceFactory
    {
        private const string codeSerialPairPattern =
            @"([\w\-]*);([A-F0-9]+:[A-F0-9]+:[A-F0-9]+:[A-F0-9]+:[A-F0-9]+:[A-F0-9]+)";

        public ModBusDevice CreateDevice(byte address, string codeSerialPair)
        {
            if (string.IsNullOrEmpty(codeSerialPair)) throw new InvalidArgumentException(nameof(codeSerialPair));

            if (address < ModBusDevice.MinAddress || address > ModBusDevice.MaxAddress)
                throw new InvalidArgumentException(nameof(address));

            var match = Regex.Match(codeSerialPair, codeSerialPairPattern);

            if (match.Success == false || match.Groups[1].Success == false)
                throw new InvalidArgumentException(nameof(codeSerialPair));

            string code = match.Groups[1].Value;
            string serial = match.Groups[2].Value;

            return code switch
                {
                    DeviceNames.TPIB19 => new TPIDevice(address, code, serial),
                    DeviceNames.TPIM19 => new TPIDevice(address, code, serial),
                    DeviceNames.TPI319 => new TPIDevice(address, code, serial),
                    DeviceNames.TPIB23 => new TPIDevice(address, code, serial),
                    DeviceNames.TPIM23 => new TPIDevice(address, code, serial),
                    DeviceNames.TPI323 => new TPIDevice(address, code, serial),
                    DeviceNames.AU04 => new AUDevice(address, code, serial),
                    DeviceNames.AU08 => new AUDevice(address, code, serial),
                    DeviceNames.AU10 => new AUDevice(address, code, serial),
                    DeviceNames.AU12 => new AUDevice(address, code, serial),
                    DeviceNames.AU20 => new AUDevice(address, code, serial),
                    DeviceNames.AU24 => new AUDevice(address, code, serial),
                    _ => throw new InvalidArgumentException(nameof(code))
                };
        }
    }
}