using EsnaData.Entities;
using EsnaData.Repositories;
using EsnaMonitoring.Services.Devices;
using EsnaMonitoring.Services.Services.Databse;
using EsnaMonitoring.Services.Services.Modbus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace EsnaWeb
{


    public class TimerHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimerHostedService> _logger;
        private readonly RecordeUpdaterService _recordeUpdaterService;
        private readonly List<ModBusDevice> ModBusDevices = new List<ModBusDevice>();
        private Timer _timer;

        public TimerHostedService(RecordeUpdaterService recordeUpdaterService, ILogger<TimerHostedService> logger)
        {
            _logger = logger;

            _recordeUpdaterService = recordeUpdaterService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(3));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            // stop timer
            _timer.Change(TimeSpan.FromMinutes(5),
                TimeSpan.FromSeconds(3));

            await _recordeUpdaterService.Update().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully == false)
                    _logger.LogInformation(
                        $"Executed on {DateTime.Now} with error {task.Exception.Message}", task.Exception.Message);
                else
                    _logger.LogInformation(
                        $"Executed on {DateTime.Now} successfully.");

            });

            // start timer
            _timer.Change(TimeSpan.Zero,
                TimeSpan.FromSeconds(3));
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
                _timer?.Dispose();
        }

    }
}
