using EsnaData.DbContexts;
using EsnaData.Entities;

namespace EsnaData.Repositories
{
    public class ConfigorationRepository : BaseRepository<Configuration, long>
    {
        public ConfigorationRepository(EsnaDbContext dbContext) : base(dbContext)
        {
        }
    }
}
