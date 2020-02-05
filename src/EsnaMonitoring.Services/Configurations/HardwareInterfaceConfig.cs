using System.IO.Ports;

namespace EsnaMonitoring.Services.Configuations
{
    public class HardwareInterfaceConfig
    {
        public int BaudRate { get; set; }

        public int Timeout { get; set; }

        public int DataBits { get; set; }

        public Parity Parity { get; set; }

        public string PortName { get; set; }

        public StopBits StopBits { get; set; }
        public Mode Mode { get; internal set; }
    }
}
