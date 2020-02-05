#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace EsnaMonitoring.Services.Devices
{
    public abstract class ModBusDevice : INotifyPropertyChanged
    {
        public const int MinAddress = 1;
        public const int MaxAddress = 247;


        private static int _lastId;
        private short[]? _data;

        public abstract byte FirstRegister { get; }

        public abstract byte Offset { get; }


        protected ModBusDevice(byte unitId, string code, string macAddress)
        {
            Code = code;
            UnitId = unitId;
            Id = ++_lastId;
            MacAddress = macAddress;
        }

        public int Id { get; }

        public byte UnitId { get; protected set; }

        public string? Code { get; protected set; }

        public string? MacAddress { get; protected set; }

        public DeviceModel Model => GetModel(Code);

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
        private static DeviceModel GetModel(string? code)
        {
            if (string.IsNullOrEmpty(code))
                return DeviceModel.Unknown;
            var match = Regex.Match(code, @"(\w+)-.*");

            if (match.Success)
            {
                Enum.TryParse<DeviceModel>(match.Groups[1].Value, ignoreCase: true, out var value);
                return value;
            }
            return DeviceModel.Unknown;
        }

        public short[] Data
        {
            get
            {
                if (_data == null)
                    return Array.Empty<short>();
                return _data;
            }
            set
            {
                _data = value;
                NotifyPropertyChanged(nameof(Data));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public enum DeviceModel
        {
            Unknown, Tpi, Au
        }
    }
}
