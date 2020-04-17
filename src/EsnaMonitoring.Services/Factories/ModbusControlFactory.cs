namespace EsnaMonitoring.Services.Factories
{
    using EsnaMonitoring.Services.Configuations;

    using Microsoft.Extensions.Options;

    using ModbusUtility;

    using Mode = ModbusUtility.Mode;

    public class ModbusControlFactory : IModbusControlFactory
    {
        private readonly IConfigurationFactory _configurationFactory;

        public ModbusControlFactory(IConfigurationFactory configurationFactory)
        {
            this._configurationFactory = configurationFactory;
        }

        public IModbusControl Create()
        {
            var hardwareInterfaceConfig = _configurationFactory.GetConfiguration();
            return new ModbusControl
            {
                BaudRate = hardwareInterfaceConfig.BaudRate,
                DataBits = hardwareInterfaceConfig.DataBits,
                Mode = (Mode)hardwareInterfaceConfig.Mode,
                Parity = (Parity)hardwareInterfaceConfig.Parity,
                PortName = hardwareInterfaceConfig.PortName,
                ResponseTimeout = (int)hardwareInterfaceConfig.Timeout
            };
        }
    }
}