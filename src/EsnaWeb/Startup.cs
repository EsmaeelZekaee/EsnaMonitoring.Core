namespace EsnaWeb
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;

    using AutoMapper;

    using EsnaData.DbContexts;
    using EsnaData.Entities;
    using EsnaData.Repositories.Interfaces;

    using EsnaMonitoring.Hubs;
    using EsnaMonitoring.Services.AutoMapper;
    using EsnaMonitoring.Services.Configuations;
    using EsnaMonitoring.Services.Configuations.IO;
    using EsnaMonitoring.Services.Factories;
    using EsnaMonitoring.Services.Fakes;
    using EsnaMonitoring.Services.Services.Data;
    using EsnaMonitoring.Services.Services.Data.Interfaces;
    using EsnaMonitoring.Services.Services.Modbus;

    using MacAddressGenerator;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapDefaultControllerRoute();
                        endpoints.MapHub<ModbusHub>("/modbusHub");

                        // endpoints.MapFallbackToClientSideBlazor<Program>("index.html");
                    });

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapBlazorHub();
                        endpoints.MapFallbackToPage("/_Host");
                    });
        }

        // this method add for register with autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register repositories
            builder.RegisterAssemblyTypes(typeof(IBaseRepository<,>).Assembly).Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            // .InstancePerRequest();

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

            builder.RegisterType<ConfigurationFactory>().As<IConfigurationFactory>();

            // Register Log Service
            builder.RegisterType<ModeBusLogReaderService>().As<IModeBusLogReaderService>();
            builder.RegisterType<ModBusLogWriterService>().As<IModBusLogWriterService>().SingleInstance();



        }

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
            services.AddDbContext<EsnaDbContext>(
                options => options.UseSqlite(this.Configuration.GetConnectionString("EsnaDatabase")));

            services.AddOptions<Config>().Configure(x => { x.OUI = "07-AA-AA"; });
            services.AddOptions<HardwareInterfaceConfig>().Configure(x => { x.PortName = "COM3"; });
            services.AddAutoMapper(typeof(EsnaMapperProfile));
        }
    }
}