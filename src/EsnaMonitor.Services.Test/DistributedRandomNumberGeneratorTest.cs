using System.Collections.Generic;

namespace EsnaMonitor.Services.Test
{
    using EsnaMonitoring.Services.Services.DistributedRandom;
    using System.Linq;
    using Xunit;

    public class DistributedRandomNumberGeneratorTest
    {
        [Fact]
        public void DistributedRandomNumberGenerator_Success()
        {
            float max = 100000f;

            DistributedRandomNumberGenerator<int> generator = new DistributedRandomNumberGenerator<int>();

            Dictionary<int, double> dic = new Dictionary<int, double>
            {
                [0] = .3,
                [10] = .1,
                [-10] = .1,
                [22] = .2,
                [45] = .3
            };

            foreach (KeyValuePair<int, double> item in dic)
            {
                generator.AddNumber(item.Key, item.Value);
            }

            int sum = 0;
            for (int i = 0; i < max; i++)
            {
                sum += generator.GetDistributedRandomNumber();
            }

            double ave = dic.Sum(x => x.Key * x.Value) / dic.Sum(x => x.Value);

            float rave = sum / max;
            Assert.True(rave > ave - .3 && rave < ave + .3);
        }
    }
}
