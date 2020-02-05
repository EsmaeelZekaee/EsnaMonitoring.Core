#nullable enable
using EsnaMonitoring.Services.Devices;
using System.Windows;
using System.Windows.Controls;

namespace EsnaMonitoring.UI
{
    /// <summary>
    /// Interaction logic for TPIDeviceBox.xaml
    /// </summary>
    public partial class TPIDeviceBox : UserControl, IDeviceUI<TPIDevice>
    {
        public TPIDevice Device
        {
            get => (TPIDevice)GetValue(DeviceProperty);
            set => SetValue(DeviceProperty, value);
        }

        public static readonly DependencyProperty DeviceProperty =
            DependencyProperty.Register("Device", typeof(TPIDevice), typeof(TPIDevice));

        public TPIDeviceBox(TPIDevice? device)
        {
            if (device != null)
            {
                Device = device;
                device!.PropertyChanged += Device_PropertyChanged;
            }
            InitializeComponent();
        }

        private void Device_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Device == null)
            {
                return;
            }

            if (e.PropertyName == nameof(Device.Data))
            {
                if (Device!.Data.Length > 0)
                {
                    Device!.Value = Device.Data[0] / 10.0f;
                }
            }
        }
    }
}
