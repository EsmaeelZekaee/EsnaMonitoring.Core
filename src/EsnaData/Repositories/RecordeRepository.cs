using EsnaData.DbContexts;
using EsnaData.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EsnaData.Repositories
{
    public class RecordeRepository : BaseRepository<Recorde, long>
    {
        public RecordeRepository(EsnaDbContext dbContext) : base(dbContext)
        {
        }

        public async ValueTask<Recorde> GetLatest(long deviceId)
        {
            return await DbContext.Set<Recorde>().Where(x => x.DeviceId == deviceId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        }
    }
}
