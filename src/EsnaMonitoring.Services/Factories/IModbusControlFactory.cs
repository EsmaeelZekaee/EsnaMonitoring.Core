using MacAddressGenerator;
using ModbusUtility;
using System.Threading.Tasks;

namespace EsnaMonitoring.Services.Factories
{
    public interface IModbusControlFactory
    {
        public IModbusControl Create();
    }
}
