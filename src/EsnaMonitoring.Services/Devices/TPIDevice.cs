#nullable enable
namespace EsnaMonitoring.Services.Devices
{
    using System.Text.RegularExpressions;

    public class TPIDevice : ModBusDevice
    {
        private readonly string[] Segments = new string[6];

        private double _value;

        public TPIDevice(byte address, string code, string macAddress)
            : base(address, code, macAddress)
        {
            var groups = Regex.Match(code, @"(\w+)-(\w)-(\d+)-(\d{1})(\d{2})").Groups;

            this.Segments[0] = groups[0].Value;
            this.Segments[1] = groups[1].Value;
            this.Segments[2] = groups[2].Value;
            this.Segments[3] = groups[3].Value;
            this.Segments[4] = groups[4].Value;
            this.Segments[5] = groups[5].Value;
        }

        public override byte FirstRegister => 1;

        public double Max => double.Parse(this.Segments[5]);

        public double Min => double.Parse(this.Segments[4]);

        public string Model => this.Segments[1];

        public override byte Offset => 1;

        public string Type => this.Segments[2];

        public double Value
        {
            get => this._value;
            set
            {
                this._value = value;
                this.NotifyPropertyChanged(nameof(this.Value));
            }
        }
    }
}