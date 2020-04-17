namespace MacAddressGenerator
{
    using System;
    using System.IO;
    using System.Linq;

    using CsvHelper;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MacAddressService : IMacAddressService
    {
        private static readonly Random Random = new Random();

        private readonly Config configuration;

        private readonly ILogger<MacAddressService> logger;

        public MacAddressService(ILogger<MacAddressService> logger, IOptions<Config> configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.configuration = configuration.Value;
        }

        public string Generate()
        {
            var mac = this.GetRandomMacAddress();

            return mac;
        }

        private static OuiDefinition GetRandomOui()
        {
            var embeddedFile = typeof(MacAddressService).Assembly.GetEmbeddedResource("oui.csv");

            using (var reader = new StreamReader(embeddedFile))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = false;

                var definitions = csv.GetRecords<OuiDefinition>().ToList();

                var index = Random.Next(0, definitions.Count - 1);

                return definitions.ElementAt(index);
            }
        }

        private string GetRandomMacAddress()
        {
            var buffer = new byte[3];

            var ouiPrefix = this.configuration.OUI;

            if (string.IsNullOrWhiteSpace(ouiPrefix))
            {
                var definition = GetRandomOui();

                this.logger.LogInformation(
                    $"Using '{definition.Oui}' for manufacturer '{definition.Manufacturer}' as Organizationally Unique Identifier (OUI)");

                ouiPrefix = $"{definition.Oui}";
            }
            else
            {
                this.logger.LogInformation($"Using custom OUI {ouiPrefix}");
            }

            Random.NextBytes(buffer);

            return $"{ouiPrefix}:{BitConverter.ToString(buffer)}".Replace("-", ":");
        }
    }
}