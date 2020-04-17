namespace EsnaMonitor.Services.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using EsnaMonitoring.Services.Services.DistributedRandom;

    using Xunit;

    public class DistributedRandomNumberGeneratorTest
    {
        [Fact]
        public void DistributedRandomNumberGenerator_Success()
        {
            var max = 100000f;

            var generator = new DistributedRandomNumberGenerator<int>();

            var dic = new Dictionary<int, double>
                          {
                              [0] = .3,
                              [10] = .1,
                              [-10] = .1,
                              [22] = .2,
                              [45] = .3
                          };

            foreach (var item in dic) generator.AddNumber(item.Key, item.Value);

            var sum = 0;
            for (var i = 0; i < max; i++) sum += generator.GetDistributedRandomNumber();

            var ave = dic.Sum(x => x.Key * x.Value) / dic.Sum(x => x.Value);

            var rave = sum / max;
            Assert.True(rave > ave - .3 && rave < ave + .3);
        }
    }
}