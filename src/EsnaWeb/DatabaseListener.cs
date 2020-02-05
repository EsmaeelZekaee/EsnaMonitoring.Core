using EsnaMonitoring.Services.Services.Databse;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsnaWeb
{

    public class DatabaseListener : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseListener> _logger;
        private readonly IHubContext<ModbusHub, ISendMessage> _modBusHub;

        public DatabaseListener(IServiceProvider serviceProvider, ILogger<DatabaseListener> logger, IHubContext<ModbusHub, ISendMessage> clockHub)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _modBusHub = clockHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {Time}", DateTime.Now);
                var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
                var devices = await dataService.GetDevicesAsync();
                foreach (var device in devices)
                {
                    await _modBusHub.Clients.All.SendMessage(device.UnitId,
                        device.Code,
                        device.MacAddress,
                        device.Data);
                }
                await Task.Delay(1000);
            }
        }
    }
}
