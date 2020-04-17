namespace EsnaData.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using EsnaData.DbContexts;
    using EsnaData.Entities;
    using EsnaData.Repositories.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class RecordeRepository : BaseRepository<Recorde, long>, IRecordeRepository
    {
        public RecordeRepository(EsnaDbContext dbContext)
            : base(dbContext)
        {
        }

        public async ValueTask<Recorde> GetLatest(long deviceId)
        {
            return await this.DbContext.Set<Recorde>().Where(x => x.DeviceId == deviceId).OrderByDescending(x => x.Id)
                       .FirstOrDefaultAsync();
        }
    }
}