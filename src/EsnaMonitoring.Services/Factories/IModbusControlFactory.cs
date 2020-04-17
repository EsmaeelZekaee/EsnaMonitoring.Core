namespace EsnaMonitoring.Services.Factories
{
    using ModbusUtility;

    public interface IModbusControlFactory
    {
        public IModbusControl Create();
    }
}