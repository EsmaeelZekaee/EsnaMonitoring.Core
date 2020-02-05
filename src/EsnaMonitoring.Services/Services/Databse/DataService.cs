namespace EsnaMonitoring.Services.Services.Databse
{
    using EsnaData.Entities;
    using EsnaData.Repositories;
    using EsnaMonitoring.Services.Devices;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataService
    {
        private readonly DeviceRepository _deviceRepository;
        private readonly RecordeRepository _recordeRepository;
        private readonly ConfigorationRepository _configorationRepository;

        public DataService(
            DeviceRepository deviceRepository,
            RecordeRepository recordeRepository,
            ConfigorationRepository configorationRepository)
        {
            _deviceRepository = deviceRepository;
            _recordeRepository = recordeRepository;
            _configorationRepository = configorationRepository;
        }
        public async Task<IEnumerable<ModBusDevice>> GetDevicesAsync()
        {
            var list = new List<ModBusDevice>();
            foreach (var device in await _deviceRepository.GetActiveDevicesAsync())
            {
                var modBusDevice = ModBusDevice.CreateDevice(device.UnitId, device.Code, device.MacAddress);
                modBusDevice.Data = device.Recordes.FirstOrDefault()?.ShortData;
                list.Add(modBusDevice);
            }
            return list;
        }
    }
}
