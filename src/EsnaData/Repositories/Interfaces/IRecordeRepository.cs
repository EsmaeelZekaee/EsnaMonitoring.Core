namespace EsnaData.Repositories.Interfaces
{
    using System.Threading.Tasks;

    using EsnaData.Entities;

    public interface IRecordeRepository : IBaseRepository<Recorde, long>
    {
        ValueTask<Recorde> GetLatest(long deviceId);
    }
}