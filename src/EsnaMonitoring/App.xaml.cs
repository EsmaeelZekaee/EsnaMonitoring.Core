using EsnaMonitoring.Factories;
using EsnaMonitoring.Services.Configuations;
using EsnaMonitoring.Services.Configuations.IO;
using EsnaMonitoring.Services.Factories;
using EsnaMonitoring.Services.Fakes;
using EsnaMonitoring.Services.Services.Modbus;
using MacAddressGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EsnaMonitoring
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<IModbusControlFactory, FakeModbusControlFactory>();
            services.AddSingleton<IMacAddressService, MacAddressService>();
            services.AddSingleton<IFileReader, FileReader>();
            services.AddSingleton<IModbusService, ModbusService>();
            services.AddSingleton<IDeviceFactory, DeviceFactory>();
            services.AddSingleton<IDeviceUIFactory, DeviceUIFactory>();
            var builder = new ConfigurationBuilder().Build();

            services.AddOptions<Config>().Configure((x) =>
            {
                x.OUI = "07-AA-AA";
            });

            services.AddOptions<HardwareInterfaceConfig>().Configure((x) =>
            {
                x.PortName = "COM3";
            });

            services.AddLogging();
        }

        private void AppOnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
