namespace EsnaWeb
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using EsnaMonitoring.Services.Factories;
    using MacAddressGenerator;
    using EsnaMonitoring.Services.Configuations.IO;
    using EsnaMonitoring.Services.Services.Modbus;
    using EsnaMonitoring.Services.Configuations;
    using EsnaData.DbContexts;
    using Microsoft.EntityFrameworkCore;
    using EsnaMonitoring.Services.Services.Data;
    using EsnaData.Repositories.Interfaces;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using EsnaMonitoring.Services.Services.Data.Interfaces;
    using EsnaData.Entities;
    using EsnaMonitoring.Hubs;
    using EsnaMonitoring.Services.Fakes;

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
            var container = new ContainerBuilder();
            container.Populate(services);

            services.AddRazorPages();
            services.AddServerSideBlazor();

            // register hosted service
            services.AddHostedService<DeviceCollectorHostedSerive>();

            // add signalR
            services.AddSignalR();

            // add DbContext
            services.AddDbContext<EsnaDbContext>(options =>
              options.UseSqlite(Configuration.GetConnectionString("EsnaDatabase")));

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

        // this method add for register with autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register repositories

            builder.RegisterAssemblyTypes(typeof(IBaseRepository<,>).Assembly)
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
                //.InstancePerRequest();

            // Register Modbus service
            builder.RegisterType<FakeModbusControlFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MacAddressService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<FileReader>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ModbusService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DeviceFactory>().AsImplementedInterfaces().SingleInstance();
         
            builder.RegisterType<EntityService<Device>>().AsImplementedInterfaces();
            builder.RegisterType<EntityService<Configuration>>().AsImplementedInterfaces();
            builder.RegisterType<EntityService<Command>>().AsImplementedInterfaces();
            builder.RegisterType<EntityService<Recorde>>().AsImplementedInterfaces();

            // Register Log Service
            builder.RegisterType<ModeBusLogReaderService>().As<IModeBusLogReaderService>();
            builder.RegisterType<ModBusLogWriterService>().As<IModBusLogWriterService>().SingleInstance();

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
