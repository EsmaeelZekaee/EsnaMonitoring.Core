#nullable enable
using EsnaMonitoring.Services.Devices;
using EsnaMonitoring.Services.Services.DistributedRandom;
using MacAddressGenerator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EsnaMonitoring.Services.Fakes
{

    public class FackeDevicesCollection
    {


        public Dictionary<byte, ModBusDevice> Devices { get; set; }
        public Dictionary<byte, List<short>> Data { get; set; }
        public Dictionary<byte, int> DeviceCounter { get; set; } = new Dictionary<byte, int>();
        public Dictionary<byte, short[]> RandomData { get; set; } = new Dictionary<byte, short[]>();


        public FackeDevicesCollection(IMacAddressService macAddressService)
        {

            Devices = new Dictionary<byte, ModBusDevice>();
            byte i = 1;


            Devices[i] = new AUDevice(i++, DeviceNames.AU04, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU04, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU04, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU04, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU08, macAddressService.Generate());
            //Devices[i] = new AUDevice(i++, DeviceNames.AU08, macAddressService.Generate());
            //Devices[i] = new AUDevice(i++, DeviceNames.AU08, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU08, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU10, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU10, macAddressService.Generate());
            //Devices[i] = new AUDevice(i++, DeviceNames.AU10, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU10, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU12, macAddressService.Generate());
            //Devices[i] = new AUDevice(i++, DeviceNames.AU20, macAddressService.Generate());
            //Devices[i] = new AUDevice(i++, DeviceNames.AU20, macAddressService.Generate());
            //Devices[i] = new AUDevice(i++, DeviceNames.AU20, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU20, macAddressService.Generate());
            //Devices[i] = new AUDevice(i++, DeviceNames.AU20, macAddressService.Generate());
            //Devices[i] = new AUDevice(i++, DeviceNames.AU24, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU24, macAddressService.Generate());
            Devices[i] = new AUDevice(i++, DeviceNames.AU24, macAddressService.Generate());
            //Devices[i] = new AUDevice(i++, DeviceNames.AU24, macAddressService.Generate());
            //Devices[i] = new AUDevice(i++, DeviceNames.AU24, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIB19, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIB19, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIB19, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIB19, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIB19, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIM19, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIM19, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIM19, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIM19, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPI319, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPI319, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPI319, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPI319, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIB23, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIB23, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIB23, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIM23, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPIM23, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPI323, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPI323, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPI323, macAddressService.Generate());
            //Devices[i] = new TPIDevice(i++, DeviceNames.TPI323, macAddressService.Generate());
            Devices[i] = new TPIDevice(i++, DeviceNames.TPI323, macAddressService.Generate());
            Devices[i] = new TPIDevice(i++, DeviceNames.TPI323, macAddressService.Generate());

            Data = new Dictionary<byte, List<short>>();

            i = 1;
            foreach (byte item in Devices.Keys)
            {
                Data[i] = new List<short>();

                if (Devices[item] is TPIDevice)
                {
                    DeviceCounter[i] = -1;
                    RandomData[i] = RandomTerrarain(new Random(DateTime.Now.Millisecond / i), 1, 19, 100, 3, 5, 1, 6, 5).Select(x => (short)(x * 10)).ToArray();
                }
                i++;
            }
        }

        public IEnumerable<short> ReadData(byte unitid)
        {
            var device = this[unitid];

            return (device?.Code) switch
            {
                DeviceNames.AU04 => GetRandomValues(unitid, 04, new[] { 0, 70, 1, 15, 2, 15 }),
                DeviceNames.AU08 => GetRandomValues(unitid, 08, new[] { 0, 90, 1, 15, 2, 15 }),
                DeviceNames.AU10 => GetRandomValues(unitid, 10, new[] { 0, 60, 1, 15, 2, 15 }),
                DeviceNames.AU12 => GetRandomValues(unitid, 12, new[] { 0, 50, 1, 15, 2, 15 }),
                DeviceNames.AU20 => GetRandomValues(unitid, 16, new[] { 0, 30, 1, 15, 2, 15 }),
                DeviceNames.AU24 => GetRandomValues(unitid, 24, new[] { 0, 20, 1, 15, 2, 15 }),
                DeviceNames.TPIB19 => GetRandomValue(unitid),
                DeviceNames.TPIM19 => GetRandomValue(unitid),
                DeviceNames.TPI319 => GetRandomValue(unitid),
                DeviceNames.TPIB23 => GetRandomValue(unitid),
                DeviceNames.TPIM23 => GetRandomValue(unitid),
                DeviceNames.TPI323 => GetRandomValue(unitid),
                _ => throw new Exception($"invalid unit id"),
            };
        }

        private IEnumerable<short> GetRandomValue(byte unitid)
        {
            DeviceCounter[unitid]++;
            if (DeviceCounter[unitid] >= RandomData[unitid].Length)
                DeviceCounter[unitid] = 0;
            yield return RandomData[unitid][DeviceCounter[unitid]];
        }




        // Define other methods and classes here
        private double[] RandomTerrarain(Random rnd, int ofset, int maximum, int length, int sinuses, int cosinuses, double amplsin, double amplcos, double noise)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", length, "Length must be greater than zero!");
            double[] returnValues = new double[length];

            for (int i = 0; i < length; i++)
            {
                // sin
                for (int sin = 1; sin <= sinuses; sin++)
                {
                    returnValues[i] += amplsin * Math.Sin((2 * sin * i * Math.PI) / (double)length);
                }
                // cos
                for (int cos = 1; cos <= cosinuses; cos++)
                {
                    returnValues[i] += amplcos * Math.Cos((2 * cos * i * Math.PI) / (double)length);
                }
                // noise
                returnValues[i] += (noise * rnd.NextDouble()) - (noise * rnd.NextDouble());
            }
            // give offset so it be higher than 0
            double ofs = returnValues.Min();
            if (ofs < ofset)
            {
                ofs *= -1;
                for (int i = 0; i < length; i++)
                {
                    returnValues[i] += ofs;
                }
            }
            // resize to be fit in 100
            double max = returnValues.Max();
            if (max >= maximum)
            {
                double scaler = max / maximum;
                for (int i = 0; i < length; i++)
                {
                    returnValues[i] /= scaler;
                }
            }
            return returnValues;
        }


        private static IEnumerable<short> GetRandomValue(byte unitId, double @default,
            int p,
            int min,
            int max)
        {

            if (p < 0 || p > 100)
            {
                throw new Exception("Invalid Probability");
            }

            double step = .5;

            DistributedRandomNumberGenerator<double> _doubleDistributedRandom = new DistributedRandomNumberGenerator<double>();

            double totals = (max - min) / step;

            for (double i = min; i < max; i += step)
            {
                _doubleDistributedRandom.AddNumber(i, (100 - p) / totals);
            }

            _doubleDistributedRandom.AddNumber(@default, p);

            yield return (short)(_doubleDistributedRandom.GetDistributedRandomNumber() * 10);
        }

        private IEnumerable<short> GetRandomValues(byte unitid,
            int len,
            int[] posibleValues)
        {
            DistributedRandomNumberGenerator<short> _intDistributedRandom = new DistributedRandomNumberGenerator<short>();

            for (int i = 0; i < posibleValues.Length; i += 2)
            {
                _intDistributedRandom.AddNumber((short)posibleValues[i], posibleValues[i + 1]);
            }

            for (int i = 0; i < len; i++)
            {
                short data = _intDistributedRandom.GetDistributedRandomNumber();
                Data[unitid].Add(data);
                yield return data;
            }
        }

        public ModBusDevice? this[byte index]
        {
            get
            {
                if (Devices.ContainsKey(index))
                {
                    return Devices[index];
                }
                return default;
            }
        }
    }
}
