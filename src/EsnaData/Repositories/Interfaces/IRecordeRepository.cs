using EsnaData.Entities;
using System.Threading.Tasks;

namespace EsnaData.Repositories.Interfaces
{
    public interface IRecordeRepository : IBaseRepository<Recorde, long>
    {
        ValueTask<Recorde> GetLatest(long deviceId);
    }
}