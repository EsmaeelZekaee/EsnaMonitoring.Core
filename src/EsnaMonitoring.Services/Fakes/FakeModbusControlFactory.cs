namespace EsnaMonitoring.Services.Fakes
{
    using EsnaMonitoring.Services.Configuations;
    using EsnaMonitoring.Services.Factories;

    using MacAddressGenerator;

    using Microsoft.Extensions.Options;

    using ModbusUtility;

    using Mode = ModbusUtility.Mode;

    public class FakeModbusControlFactory : IModbusControlFactory
    {
        private readonly IOptions<HardwareInterfaceConfig> _hardwareInterfaceConfig;

        private readonly IMacAddressService _macAddressService;

        public FakeModbusControlFactory(
            IMacAddressService macAddressService,
            IOptions<HardwareInterfaceConfig> hardwareInterfaceConfig)
        {
            this._macAddressService = macAddressService;
            this._hardwareInterfaceConfig = hardwareInterfaceConfig;
        }

        public IModbusControl Create()
        {
            var hardwareInterfaceConfig = this._hardwareInterfaceConfig.Value;
            return new FakeModbusControl(new FackeDevicesCollection(this._macAddressService))
                       {
                           BaudRate = hardwareInterfaceConfig.BaudRate,
                           DataBits = hardwareInterfaceConfig.DataBits,
                           Mode = (Mode)hardwareInterfaceConfig.Mode,
                           Parity = (Parity)hardwareInterfaceConfig.Parity,
                           PortName = hardwareInterfaceConfig.PortName,
                           ResponseTimeout = hardwareInterfaceConfig.Timeout
                       };
        }
    }
}