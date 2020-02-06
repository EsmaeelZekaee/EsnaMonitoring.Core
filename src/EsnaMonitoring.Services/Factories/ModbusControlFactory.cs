using EsnaMonitoring.Services.Configuations;
using Microsoft.Extensions.Options;
using ModbusUtility;

namespace EsnaMonitoring.Services.Factories
{
    public class ModbusControlFactory : IModbusControlFactory
    {
        private readonly IOptions<HardwareInterfaceConfig> _hardwareInterfaceConfig;

        public ModbusControlFactory(IOptions<HardwareInterfaceConfig> hardwareInterfaceConfig)
        {
            _hardwareInterfaceConfig = hardwareInterfaceConfig;
        }


        public IModbusControl Create()
        {
            var hardwareInterfaceConfig = _hardwareInterfaceConfig.Value;
            return new ModbusControl()
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

