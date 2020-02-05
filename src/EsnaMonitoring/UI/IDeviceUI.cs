using EsnaMonitoring.Services.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace EsnaMonitoring.UI
{
    public interface IDeviceUI<T>
        where T : Device
    {
        T Device { get; set; }
    }
}
