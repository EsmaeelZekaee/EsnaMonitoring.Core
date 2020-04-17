namespace EsnaMonitoring.Services.Services.Modbus
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using EsnaMonitoring.Services.Devices;
    using EsnaMonitoring.Services.Factories;
    using EsnaMonitoring.Services.Services.Modbus.Interfaces;

    using ModbusUtility;

    public class ModbusService : IModbusService
    {
        private const int MaxAddress = 0xF7;

        private readonly IDeviceFactory _deviceFactory;

        private readonly IModbusControlFactory _modbusControlFactory;

        public ModbusService(IModbusControlFactory modbusControlFactory, IDeviceFactory deviceFactory)
        {
            this._deviceFactory = deviceFactory;
            this._modbusControlFactory = modbusControlFactory;
        }

        public IModbusControl ModbusControl { get; private set; }

        public void Connect()
        {
            this.ModbusControl = this._modbusControlFactory.Create();
            this.ModbusControl.OpenAsync().GetAwaiter();
        }

        public async Task ConnectAsync()
        {
            this.ModbusControl = this._modbusControlFactory.Create();
            await this.ModbusControl.OpenAsync();
        }

        public void Disconnect()
        {
            this.ModbusControl.Close();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public async IAsyncEnumerable<ModBusDevice> GetDevicesAsync()
        {
            for (byte i = 1; i < MaxAddress; i++)
            {
                var result = await this.ModbusControl.DetectDeviceAsync(i);
                Thread.Sleep(10);
                if (result.Result == Result.SUCCESS && string.IsNullOrEmpty(result.Data) == false)
                    yield return this._deviceFactory.CreateDevice(i, result.Data);
            }
        }

        public ValueTask<short[]> UpdateDeviceAsync(ModBusDevice device)
        {
            return this.UpdateDeviceAsync(device.UnitId, device.FirstRegister, device.Offset);
        }

        public async ValueTask<short[]> UpdateDeviceAsync(byte unitId, byte firstRegister, byte offset)
        {
            var result = await this.ModbusControl.ReadHoldingRegistersAsync(unitId, firstRegister, offset);
            Thread.Sleep(10);
            return result.Data;
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose) this.ModbusControl.Dispose();
        }
    }
}