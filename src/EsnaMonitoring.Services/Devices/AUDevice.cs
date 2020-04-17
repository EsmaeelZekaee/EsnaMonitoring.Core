#nullable enable
namespace EsnaMonitoring.Services.Devices
{
    using System.Text.RegularExpressions;

    public class AUDevice : ModBusDevice
    {
        private readonly string[] Segments = new string[4];

        public AUDevice(byte unitId, string code, string macAddress)
            : base(unitId, code, macAddress)
        {
            var groups = Regex.Match(code, @"(\w+)-(\d{2})W-(\w)").Groups;
            this.Segments[0] = groups[0].Value;
            this.Segments[1] = groups[1].Value;
            this.Segments[2] = groups[2].Value;
            this.Segments[3] = groups[3].Value;
        }

        public override byte FirstRegister => 1;

        public override byte Offset => this.WindowsCount;

        public short[]? Windows => this.Data;

        public byte WindowsCount => byte.Parse(this.Segments[2]);
    }
}