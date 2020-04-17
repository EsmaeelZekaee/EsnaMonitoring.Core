

namespace EsnaMonitoring.Services.Factories
{
    using System.Threading.Tasks;

    using EsnaMonitoring.Services.Models;

    public interface IConfigurationFactory
    {
        Task<ConfigurationModel> GetConfigurationAsync();
        ConfigurationModel GetConfiguration();
    }
}
