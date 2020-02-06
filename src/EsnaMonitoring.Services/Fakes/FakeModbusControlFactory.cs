using EsnaMonitoring.Services.Configuations;
using EsnaMonitoring.Services.Factories;
using MacAddressGenerator;
using Microsoft.Extensions.Options;
using ModbusUtility;

namespace EsnaMonitoring.Services.Fakes
{
    public class FakeModbusControlFactory : IModbusControlFactory
    {
        private readonly IMacAddressService _macAddressService;
        private readonly IOptions<HardwareInterfaceConfig> _hardwareInterfaceConfig;

        public FakeModbusControlFactory(IMacAddressService
            macAddressService,
            IOptions<HardwareInterfaceConfig> hardwareInterfaceConfig)
        {
            _macAddressService = macAddressService;
            _hardwareInterfaceConfig = hardwareInterfaceConfig;
        }

        public IModbusControl Create()
        {
            var hardwareInterfaceConfig = _hardwareInterfaceConfig.Value;
            return new FakeModbusControl(new FackeDevicesCollection(_macAddressService))
            {
                BaudRate = hardwareInterfaceConfig.BaudRate,
                DataBits = hardwareInterfaceConfig.DataBits,
                Mode = (ModbusUtility.Mode)hardwareInterfaceConfig.Mode,
                Parity = (Parity)hardwareInterfaceConfig.Parity,
                PortName = hardwareInterfaceConfig.PortName,
                ResponseTimeout = hardwareInterfaceConfig.Timeout
            };
        }
    }
}
