using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Testura.Android.Device.Configurations;

namespace Testura.Android.Device
{
    public class AndroidDeviceFactory
    {
        private readonly TimeSpan _maxWaitTime;
        private readonly TimeSpan _timeBetweenChecks;
        private static readonly IList<DeviceConfiguration> _busyDevices;

        static AndroidDeviceFactory()
        {
            _busyDevices = new List<DeviceConfiguration>();
        }

        public AndroidDeviceFactory()
        {
            _maxWaitTime = TimeSpan.FromMinutes(360);
            _timeBetweenChecks = TimeSpan.FromSeconds(20);
        }

        public AndroidDeviceFactory(TimeSpan maxWaitTime, TimeSpan timeBetweenChecks)
        {
            _maxWaitTime = maxWaitTime;
            _timeBetweenChecks = timeBetweenChecks;
        }

        public IAndroidDevice GetDevice(params DeviceConfiguration[] possibleDevices)
        {
            var configuration = GetConfiguration(possibleDevices);
            return new AndroidDevice(new DeviceConfiguration
            {
                AdbPath = configuration.AdbPath, 
                DependenciesDirectory = configuration.DependenciesDirectory,
                Dependencies = configuration.Dependencies,
                Port = GetPort(possibleDevices, configuration),
                Serial = configuration.Serial
            });
        }

        public void DisposeDevice(IAndroidDevice device)
        {
            lock (_busyDevices)
            {
                device.StopServer();
                var config = _busyDevices.FirstOrDefault(b => b.Serial == device.Serial);
                if (config != null)
                {
                    _busyDevices.Remove(config);
                }
            }
        }


        private DeviceConfiguration GetConfiguration(DeviceConfiguration[] possibleDevices)
        {
            var start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < _maxWaitTime.TotalMilliseconds)
            {
                lock (_busyDevices)
                {
                    if (!possibleDevices.Any())
                    {
                        if (SeeIfDeviceAreAvailable(string.Empty))
                        {
                            _busyDevices.Add(new DeviceConfiguration());
                            return null;
                        }
                    }

                    var availableDevice = possibleDevices.FirstOrDefault(p => SeeIfDeviceAreAvailable(p.Serial));
                    if (availableDevice != null)
                    {
                        _busyDevices.Add(availableDevice);
                        return availableDevice;
                    }
                }

                Thread.Sleep(_timeBetweenChecks);
            }

            throw new Exception($"Could not find any available devices after {_maxWaitTime} minutes.");
        }

        private bool SeeIfDeviceAreAvailable(string serial)
        {
            return _busyDevices.All(b => b.Serial != serial);
        }

        private int GetPort(DeviceConfiguration[] possibleDevices, DeviceConfiguration configuration)
        {
            if (possibleDevices.Select(p => p.Port).Distinct().Count() == possibleDevices.Length)
            {
                return configuration.Port;
            }

            var stripedSerial = new string(configuration.Serial.Where(char.IsDigit).ToArray());
            return (int)(double.Parse(stripedSerial) % 65535);
        }

    }
}
