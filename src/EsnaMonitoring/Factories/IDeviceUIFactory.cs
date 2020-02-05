using EsnaMonitoring.Services.Devices;
using EsnaMonitoring.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EsnaMonitoring.Factories
{
    public interface IDeviceUIFactory
    {
        UIElement Create(Device device);
    }
}
