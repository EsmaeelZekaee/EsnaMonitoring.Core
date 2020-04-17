#nullable enable
namespace EsnaMonitoring.Services.Fakes
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using ModbusUtility;

    public class FakeModbusControl : IModbusControl
    {
        private readonly FackeDevicesCollection _fackeDevicesCollection;

        public FakeModbusControl(FackeDevicesCollection fackeDevicesCollection)
        {
            this._fackeDevicesCollection = fackeDevicesCollection;
        }

        public int BaudRate { get; set; }

        public int DataBits { get; set; }

        public bool DTREnable { get; set; }

        public bool IsOpen { get; private set; }

        public Mode Mode { get; set; }

        public Parity Parity { get; set; }

        public string? PortName { get; set; }

        public bool RemoveEcho { get; set; }

        public int ResponseTimeout { get; set; }

        public bool RTSEnable { get; set; }

        public int StopBits { get; set; }

        public void Close()
        {
            this.IsOpen = false;
        }

        public Result DetectDevice(byte unitId, out string name)
        {
            throw new NotImplementedException();
        }

        public ValueTask<DataResult<string>> DetectDeviceAsync(byte unitId)
        {
            if (this.IsOpen == false)
                return new ValueTask<DataResult<string>>(new DataResult<string>(Result.ISCLOSED, null));
            var result = this._fackeDevicesCollection[unitId];

            var dataResult = new DataResult<string>();
            if (result == null)
            {
                dataResult.Result = Result.NOT_AVAILABLE;
            }
            else
            {
                dataResult.Result = Result.SUCCESS;
                dataResult.Data = $"{result.Code};{result.MacAddress}";
            }

            return new ValueTask<DataResult<string>>(dataResult);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public short[] FloatToRegisters(float value)
        {
            throw new NotImplementedException();
        }

        public string GetLastErrorString()
        {
            throw new NotImplementedException();
        }

        public string[] GetPortNames()
        {
            throw new NotImplementedException();
        }

        public int GetRxBuffer(byte[] byteArray)
        {
            throw new NotImplementedException();
        }

        public int GetTxBuffer(byte[] byteArray)
        {
            throw new NotImplementedException();
        }

        public short[] Int32ToRegisters(int value)
        {
            throw new NotImplementedException();
        }

        public Result MaskWriteRegister(byte unitId, ushort address, ushort andMask, ushort orMask)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result> OpenAsync()
        {
            Thread.Sleep(500);
            this.IsOpen = true;
            return new ValueTask<Result>(Result.SUCCESS);
        }

        public Result ReadCoils(byte unitId, ushort address, ushort quantity, bool[] coils)
        {
            throw new NotImplementedException();
        }

        public Result ReadCoils(byte unitId, ushort address, ushort quantity, bool[] coils, int offset)
        {
            throw new NotImplementedException();
        }

        public Result ReadDiscreteInputs(byte unitId, ushort address, ushort quantity, bool[] discreteInputs)
        {
            throw new NotImplementedException();
        }

        public Result ReadDiscreteInputs(
            byte unitId,
            ushort address,
            ushort quantity,
            bool[] discreteInputs,
            int offset)
        {
            throw new NotImplementedException();
        }

        public Result ReadHoldingRegisters(byte unitId, ushort address, ushort quantity, short[] registers)
        {
            throw new NotImplementedException();
        }

        public Result ReadHoldingRegisters(byte unitId, ushort address, ushort quantity, short[] registers, int offset)
        {
            throw new NotImplementedException();
        }

        public ValueTask<DataResult<short[]>> ReadHoldingRegistersAsync(byte unitId, ushort address, ushort quantity)
        {
            if (this.IsOpen == false)
                return new ValueTask<DataResult<short[]>>(new DataResult<short[]>(Result.ISCLOSED, null));
            var data = this._fackeDevicesCollection.ReadData(unitId).ToArray();
            return new ValueTask<DataResult<short[]>>(new DataResult<short[]>(Result.SUCCESS, data));
        }

        public Result ReadInputRegisters(byte unitId, ushort address, ushort quantity, short[] registers)
        {
            throw new NotImplementedException();
        }

        public Result ReadInputRegisters(byte unitId, ushort address, ushort quantity, short[] registers, int offset)
        {
            throw new NotImplementedException();
        }

        public Result ReadUserDefinedCoils(byte unitId, byte function, ushort address, ushort quantity, bool[] coils)
        {
            throw new NotImplementedException();
        }

        public Result ReadUserDefinedCoils(
            byte unitId,
            byte function,
            ushort address,
            ushort quantity,
            bool[] coils,
            int offset)
        {
            throw new NotImplementedException();
        }

        public Result ReadUserDefinedRegisters(
            byte unitId,
            byte function,
            ushort address,
            ushort quantity,
            short[] registers)
        {
            throw new NotImplementedException();
        }

        public Result ReadUserDefinedRegisters(
            byte unitId,
            byte function,
            ushort address,
            ushort quantity,
            short[] registers,
            int offset)
        {
            throw new NotImplementedException();
        }

        public Result ReadWriteMultipleRegisters(
            byte unitId,
            ushort readAddress,
            ushort readQuantity,
            short[] readRegisters,
            ushort writeAddress,
            ushort writeQuantity,
            short[] writeRegisters)
        {
            throw new NotImplementedException();
        }

        public float RegistersToFloat(short hiReg, short loReg)
        {
            throw new NotImplementedException();
        }

        public int RegistersToInt32(short hiReg, short loReg)
        {
            throw new NotImplementedException();
        }

        public Result ReportSlaveID(byte unitId, out byte byteCount, byte[] deviceSpecific)
        {
            throw new NotImplementedException();
        }

        public Result WriteMultipleCoils(byte unitId, ushort address, ushort quantity, bool[] coils)
        {
            throw new NotImplementedException();
        }

        public Result WriteMultipleCoils(byte unitId, ushort address, ushort quantity, bool[] coils, int offset)
        {
            throw new NotImplementedException();
        }

        public Result WriteMultipleRegisters(byte unitId, ushort address, ushort quantity, short[] registers)
        {
            throw new NotImplementedException();
        }

        public Result WriteMultipleRegisters(
            byte unitId,
            ushort address,
            ushort quantity,
            short[] registers,
            int offset)
        {
            throw new NotImplementedException();
        }

        public Result WriteSingleCoil(byte unitId, ushort address, bool coil)
        {
            throw new NotImplementedException();
        }

        public Result WriteSingleRegister(byte unitId, ushort address, short register)
        {
            throw new NotImplementedException();
        }

        public Result WriteUserDefinedCoils(byte unitId, byte function, ushort address, ushort quantity, bool[] coils)
        {
            throw new NotImplementedException();
        }

        public Result WriteUserDefinedCoils(
            byte unitId,
            byte function,
            ushort address,
            ushort quantity,
            bool[] coils,
            int offset)
        {
            throw new NotImplementedException();
        }

        public Result WriteUserDefinedRegisters(
            byte unitId,
            byte function,
            ushort address,
            ushort quantity,
            short[] registers)
        {
            throw new NotImplementedException();
        }

        public Result WriteUserDefinedRegisters(
            byte unitId,
            byte function,
            ushort address,
            ushort quantity,
            short[] registers,
            int offset)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool dispose)
        {
            if (!dispose)
                return;
            GC.SuppressFinalize(this);
        }
    }
}