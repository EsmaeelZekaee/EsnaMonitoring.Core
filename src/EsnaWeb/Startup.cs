using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EsnaMonitoring.Services.Factories;
using MacAddressGenerator;
using EsnaMonitoring.Services.Configuations.IO;
using EsnaMonitoring.Services.Services.Modbus;
using EsnaMonitoring.Services.Fakes;
using EsnaMonitoring.Services.Configuations;
using EsnaData.DbContexts;
using Microsoft.EntityFrameworkCore;
using EsnaData.Repositories;
using EsnaMonitoring.Services.Services.Databse;
using System.Text.Json;
using System.IO;

namespace EsnaWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<IModbusControlFactory, FakeModbusControlFactory>();
            services.AddSingleton<IMacAddressService, MacAddressService>();
            services.AddSingleton<IFileReader, FileReader>();
            services.AddSingleton<IModbusService, ModbusService>();
            services.AddSingleton<IDeviceFactory, DeviceFactory>();
            services.AddSingleton<TimerService>();
            services.AddHostedService<DatabaseListener>();
            services.AddSingleton<RecordeUpdaterService>();
            services.AddScoped<ConfigorationRepository>();
            services.AddScoped<DeviceRepository>();
            services.AddScoped<RecordeRepository>();
            services.AddScoped<DataService>();
            services.AddSignalR();
            services.AddDbContext<EsnaDbContext>(options =>
              options.UseSqlite(Configuration.GetConnectionString("EsnaDatabase")));

            services.AddHostedService<TimerHostedService>();

            var builder = new ConfigurationBuilder().Build();

            services.AddOptions<Config>().Configure((x) =>
            {
                x.OUI = "07-AA-AA";
            });

            services.AddOptions<HardwareInterfaceConfig>().Configure((x) =>
            {
                x.PortName = "COM3";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
          
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<ModbusHub>("/modbusHub");
               // endpoints.MapFallbackToClientSideBlazor<Program>("index.html");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
