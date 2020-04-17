namespace EsnaMonitoring.Services.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EsnaData.Repositories.Interfaces;

    using EsnaMonitoring.Services.Devices;
    using EsnaMonitoring.Services.Services.Data.Interfaces;

    public class ModeBusLogReaderService : IModeBusLogReaderService
    {
        private readonly IConfigorationRepository _configorationRepository;

        private readonly IDeviceRepository _deviceRepository;

        private readonly IRecordeRepository _recordeRepository;

        public ModeBusLogReaderService(
            IDeviceRepository deviceRepository,
            IRecordeRepository recordeRepository,
            IConfigorationRepository configorationRepository)
        {
            this._deviceRepository = deviceRepository;
            this._recordeRepository = recordeRepository;
            this._configorationRepository = configorationRepository;
        }

        public async Task<IEnumerable<ModBusDevice>> GetDevicesAsync()
        {
            var list = new List<ModBusDevice>();

            // foreach (var device in await _deviceRepository.GetActiveDevicesAsync())
            // {
            // var modBusDevice = ModBusDevice.CreateDevice(device.UnitId, device.Code, device.MacAddress);
            // modBusDevice.Data = device.Recordes.FirstOrDefault()?.ShortData;
            // list.Add(modBusDevice);
            // }
            return list;
        }
    }
}