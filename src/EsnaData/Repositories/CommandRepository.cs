namespace EsnaData.Repositories
{
    using EsnaData.DbContexts;
    using EsnaData.Entities;
    using EsnaData.Repositories.Interfaces;

    public class CommandRepository : BaseRepository<Command, long>, ICommandRepository
    {
        public CommandRepository(EsnaDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}