namespace EsnaMonitoring.Services.Services.Modbus
{
    using EsnaMonitoring.Services.Devices;
    using EsnaMonitoring.Services.Factories;
    using EsnaMonitoring.Services.Services.Modbus.Interfaces;
    using ModbusUtility;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class ModbusService : IModbusService
    {
        private const int MaxAddress = 0xF7;

        private readonly IDeviceFactory _deviceFactory;
        private readonly IModbusControlFactory _modbusControlFactory;

        public IModbusControl ModbusControl { get; private set; }

        public ModbusService(IModbusControlFactory modbusControlFactory,
            IDeviceFactory deviceFactory)
        {
            _deviceFactory = deviceFactory;
            _modbusControlFactory = modbusControlFactory;
        }

        public void Connect()
        {

            ModbusControl = _modbusControlFactory.Create();
            ModbusControl.OpenAsync().GetAwaiter();
        }

        public async Task ConnectAsync()
        {
            ModbusControl = _modbusControlFactory.Create();
            await ModbusControl.OpenAsync();
        }

        public async IAsyncEnumerable<ModBusDevice> GetDevicesAsync()
        {
            for (byte i = 1; i < MaxAddress; i++)
            {
                var result = await ModbusControl.DetectDeviceAsync(i);
                Thread.Sleep(10);
                if (result.Result == Result.SUCCESS && string.IsNullOrEmpty(result.Data) == false)
                    yield return _deviceFactory.CreateDevice(i, result.Data);
            }
        }

        public void Disconnect()
        {
            ModbusControl.Close();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
                ModbusControl.Dispose();
        }

        public ValueTask<short[]> UpdateDeviceAsync(ModBusDevice device)
        {
            return UpdateDeviceAsync(device.UnitId, device.FirstRegister, device.Offset);
        }

        public async ValueTask<short[]> UpdateDeviceAsync(byte unitId, byte firstRegister, byte offset)
        {
            var result = await ModbusControl.ReadHoldingRegistersAsync(unitId, firstRegister, offset);
            Thread.Sleep(10);
            return result.Data;

        }
    }
}

