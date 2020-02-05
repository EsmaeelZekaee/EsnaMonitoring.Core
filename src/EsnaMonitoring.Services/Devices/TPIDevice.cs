#nullable enable
using System.Linq;
using System.Text.RegularExpressions;

namespace EsnaMonitoring.Services.Devices
{
    public class TPIDevice : ModBusDevice
    {
        private readonly string[] Segments = new string[6];
        private double _value;

        public TPIDevice(byte address, string code, string macAddress) :
            base(address, code, macAddress)
        {
            var groups = Regex.Match(code, @"(\w+)-(\w)-(\d+)-(\d{1})(\d{2})").Groups;

            Segments[0] = groups[0].Value;
            Segments[1] = groups[1].Value;
            Segments[2] = groups[2].Value;
            Segments[3] = groups[3].Value;
            Segments[4] = groups[4].Value;
            Segments[5] = groups[5].Value;
        }

        public double Min => double.Parse(Segments[4]);

        public double Max => double.Parse(Segments[5]);

        public string Model => Segments[1];

        public string Type => Segments[2];

        public double Value
        {
            get => _value; set
            {
                _value = value;
                NotifyPropertyChanged(nameof(Value));
            }
        }


        public override byte FirstRegister => 1;

        public override byte Offset => 1;
    }
}
