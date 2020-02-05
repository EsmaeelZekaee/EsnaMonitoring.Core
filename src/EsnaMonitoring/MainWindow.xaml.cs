using EsnaMonitoring.Factories;
using EsnaMonitoring.Services.Devices;
using EsnaMonitoring.Services.Services.Modbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace EsnaMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IModbusService _modbusService;
        private readonly IDeviceUIFactory _deviceUIFactory;
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private IList<Device> Devices { get; } = new List<Device>();

        public MainWindow(IModbusService modbusService, IDeviceUIFactory deviceUIFactory)
        {
            InitializeComponent();
            _modbusService = modbusService;
            _deviceUIFactory = deviceUIFactory;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () =>
            {
                await _modbusService.ConnectAsync().ConfigureAwait(true);
                IAsyncEnumerable<Device> devices = _modbusService.GetDevicesAsync();

                Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action(async () =>
                   {
                       await foreach (Device device in devices)
                       {
                           wpDevices.Children.Add(_deviceUIFactory.Create(device));
                           device.Data = await _modbusService.UpdateDeviceAsync(device);
                           Devices.Add(device);
                       }

                       //mnDevices.ItemsSource = Devices.GroupBy(x => x.Code).Select(x => x.Key);

                   }));

            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {

                }
                _dispatcherTimer.Interval = new TimeSpan(1000);
                _dispatcherTimer.IsEnabled = true;
                _dispatcherTimer.Tick += UpdateDevices;
            }).ConfigureAwait(false);
        }

        private async void UpdateDevices(object sender, EventArgs e)
        {
            _dispatcherTimer.Tick -= UpdateDevices;
            foreach (Device device in Devices)
            {
                device.Data = await _modbusService.UpdateDeviceAsync(device);
            }
            _dispatcherTimer.Tick += UpdateDevices;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _modbusService.Disconnect();
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            tbControl.SelectedIndex = 1;
        }

        private void HomeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            tbControl.SelectedIndex = 0;
        }
    }
}
