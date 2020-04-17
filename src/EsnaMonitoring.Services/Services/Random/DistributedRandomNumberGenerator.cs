namespace EsnaMonitoring.Services.Services.DistributedRandom
{
    using System;
    using System.Collections.Generic;

    using EsnaMonitoring.Services.Services.DistributedRandom.Interfaces;

    public class DistributedRandomNumberGenerator<T> : IDistributedRandomNumberGenerator<T>
        where T : struct, IComparable<T>, IConvertible
    {
        private static readonly Random Random = new Random();

        private readonly Dictionary<T, double> _distribution;

        private double _total;

        public DistributedRandomNumberGenerator()
        {
            this._distribution = new Dictionary<T, double>();
        }

        public void AddNumber(T value, double distribution)
        {
            if (this._distribution.ContainsKey(value)) this._total -= this._distribution[value];

            this._distribution[value] = distribution;
            this._total += distribution;
        }

        public T GetDistributedRandomNumber()
        {
            double rand = Random.NextDouble();
            double ratio = 1.0f / this._total;
            double tempDist = 0;

            foreach (var item in this._distribution)
            {
                tempDist += item.Value;
                if (rand / ratio <= tempDist)
                    return item.Key;
            }

            return default;
        }
    }
}