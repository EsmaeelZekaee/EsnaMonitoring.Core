#nullable enable
using System;
using System.Threading.Tasks;

namespace ModbusUtility
{
    public interface IModbusControl : IDisposable
    {
        int BaudRate { get; set; }
        int DataBits { get; set; }
        bool DTREnable { get; set; }
        Mode Mode { get; set; }
        Parity Parity { get; set; }
        string? PortName { get; set; }
        bool RemoveEcho { get; set; }
        int ResponseTimeout { get; set; }
        bool RTSEnable { get; set; }
        int StopBits { get; set; }

        void Close();

        ValueTask<DataResult<string>> DetectDeviceAsync(byte unitId);

        Result DetectDevice(byte unitId, out string name);
        short[] FloatToRegisters(float value);
        string GetLastErrorString();
        string[] GetPortNames();
        int GetRxBuffer(byte[] byteArray);
        int GetTxBuffer(byte[] byteArray);
        short[] Int32ToRegisters(int value);
        Result MaskWriteRegister(byte unitId, ushort address, ushort andMask, ushort orMask);
        ValueTask<Result> OpenAsync();
        Result ReadCoils(byte unitId, ushort address, ushort quantity, bool[] coils);
        Result ReadCoils(byte unitId, ushort address, ushort quantity, bool[] coils, int offset);
        Result ReadDiscreteInputs(byte unitId, ushort address, ushort quantity, bool[] discreteInputs);
        Result ReadDiscreteInputs(byte unitId, ushort address, ushort quantity, bool[] discreteInputs, int offset);
        Result ReadHoldingRegisters(byte unitId, ushort address, ushort quantity, short[] registers);
        ValueTask<DataResult<short[]>> ReadHoldingRegistersAsync(byte unitId, ushort address, ushort quantity);
        Result ReadHoldingRegisters(byte unitId, ushort address, ushort quantity, short[] registers, int offset);
        Result ReadInputRegisters(byte unitId, ushort address, ushort quantity, short[] registers);
        Result ReadInputRegisters(byte unitId, ushort address, ushort quantity, short[] registers, int offset);
        Result ReadUserDefinedCoils(byte unitId, byte function, ushort address, ushort quantity, bool[] coils);
        Result ReadUserDefinedCoils(byte unitId, byte function, ushort address, ushort quantity, bool[] coils, int offset);
        Result ReadUserDefinedRegisters(byte unitId, byte function, ushort address, ushort quantity, short[] registers);
        Result ReadUserDefinedRegisters(byte unitId, byte function, ushort address, ushort quantity, short[] registers, int offset);
        Result ReadWriteMultipleRegisters(byte unitId, ushort readAddress, ushort readQuantity, short[] readRegisters, ushort writeAddress, ushort writeQuantity, short[] writeRegisters);
        float RegistersToFloat(short hiReg, short loReg);
        int RegistersToInt32(short hiReg, short loReg);
        Result ReportSlaveID(byte unitId, out byte byteCount, byte[] deviceSpecific);
        Result WriteMultipleCoils(byte unitId, ushort address, ushort quantity, bool[] coils);
        Result WriteMultipleCoils(byte unitId, ushort address, ushort quantity, bool[] coils, int offset);
        Result WriteMultipleRegisters(byte unitId, ushort address, ushort quantity, short[] registers);
        Result WriteMultipleRegisters(byte unitId, ushort address, ushort quantity, short[] registers, int offset);
        Result WriteSingleCoil(byte unitId, ushort address, bool coil);
        Result WriteSingleRegister(byte unitId, ushort address, short register);
        Result WriteUserDefinedCoils(byte unitId, byte function, ushort address, ushort quantity, bool[] coils);
        Result WriteUserDefinedCoils(byte unitId, byte function, ushort address, ushort quantity, bool[] coils, int offset);
        Result WriteUserDefinedRegisters(byte unitId, byte function, ushort address, ushort quantity, short[] registers);
        Result WriteUserDefinedRegisters(byte unitId, byte function, ushort address, ushort quantity, short[] registers, int offset);
    }
}