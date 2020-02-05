#nullable enable
using System.Text.RegularExpressions;

namespace EsnaMonitoring.Services.Devices
{
    public class AUDevice : ModBusDevice
    {
        private readonly string[] Segments = new string[4];

        public AUDevice(byte unitId, string code, string macAddress) :
            base(unitId, code, macAddress)
        {
            var groups = Regex.Match(code, @"(\w+)-(\d{2})W-(\w)").Groups;
            Segments[0] = groups[0].Value;
            Segments[1] = groups[1].Value;
            Segments[2] = groups[2].Value;
            Segments[3] = groups[3].Value;
        }

        public byte WindowsCount => byte.Parse(Segments[2]);

        public override byte FirstRegister => 1;

        public override byte Offset => WindowsCount;

        public short[]? Windows => this.Data;
    }
}
