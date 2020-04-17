namespace EsnaMonitoring.Services.Factories
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Linq;
    using System.Threading.Tasks;

    using EsnaData.Entities;

    using EsnaMonitoring.Services.Models;
    using EsnaMonitoring.Services.Services.Data.Interfaces;

    using global::AutoMapper;

    using Microsoft.EntityFrameworkCore.Internal;

    using ModbusUtility;

    using Mode = EsnaMonitoring.Services.Configuations.Mode;
    using Parity = System.IO.Ports.Parity;

    public class ConfigurationFactory : IConfigurationFactory
    {
        static ConfigurationFactory()
        {
            Parities = EnumExtension.GetValues<Parity>(typeof(Parity));
            StopBitses = EnumExtension.GetValues<StopBits>(typeof(StopBits));
            Timeouts = EnumExtension.GetValues<Timeout>(typeof(Timeout));
            PortNames = SerialPort.GetPortNames();
            Modes = EnumExtension.GetValues<Mode>(typeof(Mode));
            BaudRates = new List<int>
                            {
                                110,
                                300,
                                600,
                                1200,
                                2400,
                                4800,
                                9600,
                                14400,
                                19200,
                                38400,
                                57600,
                                115200,
                                128000,
                                256000
                            };
            DataBitses = new List<int> { 7, 8 };
        }

        public static IEnumerable<int> BaudRates { get; }

        public static IEnumerable<int> DataBitses { get; }

        public static IEnumerable<Mode> Modes { get; }

        public static IEnumerable<Parity> Parities { get; }

        public static IEnumerable<string> PortNames { get; }

        public static IEnumerable<StopBits> StopBitses { get; }

        public static IEnumerable<Timeout> Timeouts { get; set; }

        private readonly IEntityService<Configuration> _entityService;

        private readonly IMapper _mapper;

        public ConfigurationFactory(IEntityService<Configuration> entityService, IMapper mapper)
        {
            _entityService = entityService;
            this._mapper = mapper;
        }
        public async Task<ConfigurationModel> GetConfigurationAsync()
        {
            var configuration = await this._entityService.FirstOrDefaultAsync(x => x.Active);
            if (configuration == null)
                await this._entityService.FirstOrDefaultAsync();
            return this.CreateInstance(configuration);
        }

        private ConfigurationModel CreateInstance(Configuration configuration)
        {
            if (configuration == null)
                configuration = new Configuration()
                {
                    Active = true,
                    Mode = (int)Mode.RTU,
                    DataBits = 7,
                    Parity = (int)Parity.Odd,
                    BaudRate = 6900,
                    Timeout = (int)Timeout.S30,
                    PortName = PortNames.FirstOrDefault(),
                    StopBits = (int)StopBits.None,
                };
            return this._mapper.Map<ConfigurationModel>(configuration);
        }

        public ConfigurationModel GetConfiguration()
        {
            var configuration = this._entityService.FirstOrDefault(x => x.Active);
            if (configuration == null)
                this._entityService.FirstOrDefault();
            return CreateInstance(configuration);
        }
    }
}