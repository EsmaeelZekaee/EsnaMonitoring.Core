using EsnaMonitoring.Services.Configuations;
using System;
using Xunit;

namespace EsnaMonitor.Services.Test
{
    public class HardwareInterfaceConfigProviderTest
    {
        [Fact]
        public void Save_ValidData_Success()
        {
            _ = new HardwareInterfaceConfig();
        }
    }
}
