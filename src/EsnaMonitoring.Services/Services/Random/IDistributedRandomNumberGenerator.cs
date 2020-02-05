namespace EsnaMonitoring.Services.Services.DistributedRandom
{
    public interface IDistributedRandomNumberGenerator<T>
        where T : struct
    {
        void AddNumber(T value, double distribution);
        T GetDistributedRandomNumber();

    }
}