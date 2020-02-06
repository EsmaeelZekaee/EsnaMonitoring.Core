using ModbusUtility;

namespace EsnaMonitoring.Services.Factories
{
    public interface IModbusControlFactory
    {
        public IModbusControl Create();
    }
}
