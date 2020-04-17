namespace EsnaMonitoring.Services.Services.DistributedRandom.Interfaces
{
    public interface IDistributedRandomNumberGenerator<T>
        where T : struct
    {
        void AddNumber(T value, double distribution);

        T GetDistributedRandomNumber();
    }
}