using EsnaData.DbContexts;
using EsnaData.Entities;
using EsnaData.Repositories.Interfaces;

namespace EsnaData.Repositories
{
    public class CommandRepository : BaseRepository<Command, long>, ICommandRepository
    {
        public CommandRepository(EsnaDbContext dbContext) : base(dbContext)
        {
        }
    }
}
