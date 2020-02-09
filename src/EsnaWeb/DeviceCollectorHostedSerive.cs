namespace EsnaWeb
{
    using EsnaData.Entities;
    using EsnaMonitoring.Services.Devices;
    using EsnaMonitoring.Services.Services.Data.Interfaces;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeviceCollectorHostedSerive : BackgroundService
    {
        private readonly ILogger<DeviceCollectorHostedSerive> _logger;
        private readonly IModBusLogWriterService _recordeUpdaterService;

        public DeviceCollectorHostedSerive(IModBusLogWriterService recordeUpdaterService, ILogger<DeviceCollectorHostedSerive> logger)
        {
            _logger = logger;
            _recordeUpdaterService = recordeUpdaterService;
        }

        //public override async Task StartAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("Timed Hosted Service is starting.");

            
        //}

        //public override Task StopAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("Timed Hosted Service is stopping.");

        //    return Task.CompletedTask;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _recordeUpdaterService.ResetDevices();
            await _recordeUpdaterService.ConnectToDeviceAsync();
            await _recordeUpdaterService.GetDevicesAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Timed Hosted Service running.");

                await _recordeUpdaterService.UpdateAsync().ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully == false)
                        _logger.LogInformation(
                            $"Executed on {DateTime.Now} with error {task.Exception.Message}", task.Exception.Message);
                    else
                        _logger.LogInformation(
                            $"Executed on {DateTime.Now} successfully.");

                });
                Thread.Sleep(100);
            }
        }
    }
}
