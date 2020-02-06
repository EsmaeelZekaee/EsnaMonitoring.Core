using EsnaData.DbContexts;
using EsnaData.Entities;
using EsnaData.Repositories.Interfaces;

namespace EsnaData.Repositories
{
    public class ConfigorationRepository : BaseRepository<Configuration, long>, IConfigorationRepository
    {
        public ConfigorationRepository(EsnaDbContext dbContext) : base(dbContext)
        {
        }
    }
}
