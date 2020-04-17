namespace EsnaMonitoring.Services.Configuations
{
    using System.IO.Ports;

    public class HardwareInterfaceConfig
    {
        public int BaudRate { get; set; }

        public int DataBits { get; set; }

        public Mode Mode { get; internal set; }

        public Parity Parity { get; set; }

        public string PortName { get; set; }

        public StopBits StopBits { get; set; }

        public int Timeout { get; set; }
    }
}