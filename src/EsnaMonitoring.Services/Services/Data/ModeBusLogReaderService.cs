namespace EsnaMonitoring.Services.Services.Data
{
    using EsnaData.Repositories.Interfaces;
    using EsnaMonitoring.Services.Devices;
    using EsnaMonitoring.Services.Services.Data.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ModeBusLogReaderService : IModeBusLogReaderService
    {

        private readonly IDeviceRepository _deviceRepository;
        private readonly IRecordeRepository _recordeRepository;
        private readonly IConfigorationRepository _configorationRepository;

        public ModeBusLogReaderService(
            IDeviceRepository deviceRepository,
            IRecordeRepository recordeRepository,
            IConfigorationRepository configorationRepository)
        {
            _deviceRepository = deviceRepository;
            _recordeRepository = recordeRepository;
            _configorationRepository = configorationRepository;
        }
        public async Task<IEnumerable<ModBusDevice>> GetDevicesAsync()
        {
            var list = new List<ModBusDevice>();
            //foreach (var device in await _deviceRepository.GetActiveDevicesAsync())
            //{
            //    var modBusDevice = ModBusDevice.CreateDevice(device.UnitId, device.Code, device.MacAddress);
            //    modBusDevice.Data = device.Recordes.FirstOrDefault()?.ShortData;
            //    list.Add(modBusDevice);
            //}
            return list;
        }
    }
}
