using System;
using System.Collections.Generic;

namespace EsnaMonitoring.Services.Services.DistributedRandom
{
    public class DistributedRandomNumberGenerator<T> : IDistributedRandomNumberGenerator<T>
        where T : struct, IComparable<T>, IConvertible
    {
        private static Random Random = new Random();

        private readonly Dictionary<T, double> _distribution;
        private double _total;

        public DistributedRandomNumberGenerator()
        {
            _distribution = new Dictionary<T, double>();
        }

        public void AddNumber(T value, double distribution)
        {
            if (_distribution.ContainsKey(value))
            {
                _total -= _distribution[value];
            }

            _distribution[value] = distribution;
            _total += distribution;
        }

        public T GetDistributedRandomNumber()
        {
            double rand = Random.NextDouble();
            double ratio = 1.0f / _total;
            double tempDist = 0;

            foreach (var item in _distribution)
            {
                tempDist += item.Value;
                if (rand / ratio <= tempDist)
                    return item.Key;
            }

            return default;
        }
    }
}
