namespace EsnaWeb
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using EsnaMonitoring.Services.Services.Data.Interfaces;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class DeviceCollectorHostedSerive : BackgroundService
    {
        private readonly ILogger<DeviceCollectorHostedSerive> _logger;

        private readonly IModBusLogWriterService _recordeUpdaterService;

        public DeviceCollectorHostedSerive(
            IModBusLogWriterService recordeUpdaterService,
            ILogger<DeviceCollectorHostedSerive> logger)
        {
            this._logger = logger;
            this._recordeUpdaterService = recordeUpdaterService;
        }

        // public override async Task StartAsync(CancellationToken stoppingToken)
        // {
        // _logger.LogInformation("Timed Hosted Service is starting.");

        // }

        // public override Task StopAsync(CancellationToken stoppingToken)
        // {
        // _logger.LogInformation("Timed Hosted Service is stopping.");

        // return Task.CompletedTask;
        // }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this._recordeUpdaterService.ResetDevices();
            await this._recordeUpdaterService.ConnectToDeviceAsync();
            await this._recordeUpdaterService.GetDevicesAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                this._logger.LogInformation("Timed Hosted Service running.");

                await this._recordeUpdaterService.UpdateAsync().ContinueWith(
                    task =>
                        {
                            if (task.IsCompletedSuccessfully == false)
                                this._logger.LogInformation(
                                    $"Executed on {DateTime.Now} with error {task.Exception.Message}",
                                    task.Exception.Message);
                            else
                                this._logger.LogInformation($"Executed on {DateTime.Now} successfully.");
                        });
                Thread.Sleep(100);
            }
        }
    }
}