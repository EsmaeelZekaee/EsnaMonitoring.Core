#nullable enable
using EsnaMonitoring.Services.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EsnaMonitoring.UI
{
    /// <summary>
    /// Interaction logic for AUDeviceBox.xaml
    /// </summary>
    public partial class AUDeviceBox : UserControl, IDeviceUI<AUDevice>
    {
        static Brush[] WindowBrushes = new Brush[] { Brushes.Gray, Brushes.Yellow, Brushes.Red };
        private bool IsInitailized = false;
        public AUDevice Device
        {
            get { return (AUDevice)GetValue(DeviceProperty); }
            set { SetValue(DeviceProperty, value); }
        }

        public static readonly DependencyProperty DeviceProperty =
            DependencyProperty.Register("Device", typeof(AUDevice), typeof(AUDevice));

        public AUDeviceBox(AUDevice? device)
        {
            if (device != null)
            {
                device!.PropertyChanged += Device_PropertyChanged;
                Device = device;
            }
            InitializeComponent();
        }

        private void Device_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Device.Data))
            {
                if (gWindows.Children.Count != Device.Windows?.Length)
                    return;
                (int rows, int cols) = GetRowsAndCols(Device.WindowsCount);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (gWindows.Children[i * cols + j] is Rectangle rec)
                        {
                            rec.Fill = WindowBrushes[Device.Windows![i * cols + j]];
                        }
                    }
                }
            }
        }

        private (int row, int cols) GetRowsAndCols(int windows)
        {
            var sums = new List<(int row, int col, int sum)>();
            for (var i = 1; i <= windows / 2; i++)
            {

                if (windows % i == 0)
                {
                    sums.Add((windows / i, i, windows / i + i));
                }
            }
            return sums.OrderBy(x => x.sum).Select(x => (x.row, x.col)).First();
        }

        private void AUDevice_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsInitailized == true)
                return;
            IsInitailized = true;
            (int rows, int cols) = GetRowsAndCols(Device.WindowsCount);
            for (int i = 0; i < rows; i++)
                gWindows.RowDefinitions.Add(new RowDefinition());
            for (int j = 0; j < cols; j++)
                gWindows.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var rec = new Rectangle()
                    {
                        Fill = WindowBrushes[Device.Windows![i * cols + j]],
                        Margin = new Thickness(2),
                        Width = (this.Width - 100) / cols,
                        Height = (this.Height - 100) / rows,
                    };
                    Grid.SetRow(rec, i);
                    Grid.SetColumn(rec, j);
                    gWindows.Children.Add(rec);
                }
            }
        }
    }
}
