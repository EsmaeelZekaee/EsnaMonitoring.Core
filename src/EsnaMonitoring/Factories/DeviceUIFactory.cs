#nullable enable
using EsnaMonitoring.Services.Devices;
using EsnaMonitoring.UI;
using System;
using System.Windows;

namespace EsnaMonitoring.Factories
{
    public class DeviceUIFactory : IDeviceUIFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DeviceUIFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public UIElement Create(Device? device)
        {
            if (device == null)
            {
                throw new Exception($"Invalid argumnet {nameof(device)}");
            }

            return device switch
            {
                AUDevice d => Create(d),
                TPIDevice d => Create(d),
                _ => throw new Exception($"Invalid Device {device.Code}"),
            };
        }

        private UIElement Create(AUDevice device)
        {
            return new AUDeviceBox(device);
        }

        private UIElement Create(TPIDevice device)
        {
            return new TPIDeviceBox(device);
        }
    }
}
