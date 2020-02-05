using CsvHelper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MacAddressGenerator
{
    public interface IMacAddressService
    {
        string Generate();
    }

    public class MacAddressService : IMacAddressService
    {
        private static readonly Random Random = new Random();

        private readonly ILogger<MacAddressService> logger;

        private readonly Config configuration;

        public MacAddressService(ILogger<MacAddressService> logger, IOptions<Config> configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.configuration = configuration.Value;
        }

        public string Generate()
        {
            string mac = GetRandomMacAddress();

            return mac;
        }

        private string GetRandomMacAddress()
        {
            byte[] buffer = new byte[3];

            string ouiPrefix = configuration.OUI;

            if (string.IsNullOrWhiteSpace(ouiPrefix))
            {
                OuiDefinition definition = GetRandomOui();

                logger.LogInformation($"Using '{definition.Oui}' for manufacturer '{definition.Manufacturer}' as Organizationally Unique Identifier (OUI)");

                ouiPrefix = $"{definition.Oui}";
            }
            else
            {
                logger.LogInformation($"Using custom OUI {ouiPrefix}");
            }
            Random.NextBytes(buffer);

            return $"{ouiPrefix}:{BitConverter.ToString(buffer)}".Replace("-", ":");
        }

        private static OuiDefinition GetRandomOui()
        {
            Stream embeddedFile = typeof(MacAddressService).Assembly.GetEmbeddedResource("oui.csv");

            using (StreamReader reader = new StreamReader(embeddedFile))
            using (CsvReader csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = false;

                List<OuiDefinition> definitions = csv.GetRecords<OuiDefinition>().ToList();

                int index = Random.Next(0, definitions.Count - 1);

                return definitions.ElementAt(index);
            }
        }
    }
}
