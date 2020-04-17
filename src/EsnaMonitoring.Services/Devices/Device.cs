#nullable enable
namespace EsnaMonitoring.Services.Devices
{
    using System;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    public abstract class ModBusDevice : INotifyPropertyChanged
    {
        public const int MaxAddress = 247;

        public const int MinAddress = 1;

        private static int _lastId;

        private short[]? _data;

        protected ModBusDevice(byte unitId, string code, string macAddress)
        {
            this.Code = code;
            this.UnitId = unitId;
            this.Id = ++_lastId;
            this.MacAddress = macAddress;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public enum DeviceModel
        {
            Unknown,

            Tpi,

            Au
        }

        public string? Code { get; protected set; }

        public short[] Data
        {
            get
            {
                if (this._data == null)
                    return Array.Empty<short>();
                return this._data;
            }

            set
            {
                this._data = value;
                this.NotifyPropertyChanged(nameof(this.Data));
            }
        }

        public abstract byte FirstRegister { get; }

        public int Id { get; }

        public string? MacAddress { get; protected set; }

        public DeviceModel Model => GetModel(this.Code);

        public abstract byte Offset { get; }

        public byte UnitId { get; protected set; }

        public static ModBusDevice CreateDevice(byte unitId, string code, string macAddress)
        {
            switch (GetModel(code))
            {
                case DeviceModel.Au:
                    return new AUDevice(unitId, code, macAddress);
                case DeviceModel.Tpi:
                    return new AUDevice(unitId, code, macAddress);
                default:
                    throw new Exception($"Invalid code {code}.");
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static DeviceModel GetModel(string? code)
        {
            if (string.IsNullOrEmpty(code))
                return DeviceModel.Unknown;
            var match = Regex.Match(code, @"(\w+)-.*");

            if (match.Success)
            {
                Enum.TryParse<DeviceModel>(match.Groups[1].Value, true, out var value);
                return value;
            }

            return DeviceModel.Unknown;
        }
    }
}