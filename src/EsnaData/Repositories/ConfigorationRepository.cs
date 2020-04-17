namespace EsnaData.Repositories
{
    using EsnaData.DbContexts;
    using EsnaData.Entities;
    using EsnaData.Repositories.Interfaces;

    public class ConfigorationRepository : BaseRepository<Configuration, long>, IConfigorationRepository
    {
        public ConfigorationRepository(EsnaDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}