using Testura.Android.Device.Configurations;
using Testura.Android.Util;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Device
{
    /// <summary>
    /// Provides functionality to create and handle multiple possible devices.
    /// </summary>
    public class AndroidDeviceFactory
    {
        private static readonly IList<(DeviceConfiguration configuration, DateTime initialized)> BusyDevices;
        private readonly TimeSpan _maxWaitTime;
        private readonly TimeSpan _timeBetweenChecks;
        private readonly TimeSpan? _releaseDeviceAfter;

        static AndroidDeviceFactory()
        {
            BusyDevices = new List<(DeviceConfiguration configuration, DateTime initialized)>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDeviceFactory"/> class.
        /// </summary>
        public AndroidDeviceFactory()
        {
            _maxWaitTime = TimeSpan.FromMinutes(360);
            _timeBetweenChecks = TimeSpan.FromSeconds(20);
            _releaseDeviceAfter = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDeviceFactory"/> class.
        /// </summary>
        /// <param name="maxWaitTime">Max time to wait until a device get available</param>
        /// <param name="timeBetweenChecks">How long we should wait before check if a device is available again</param>
        /// <param name="releaseDeviceAfter">If we should release device if x time has passed</param>
        public AndroidDeviceFactory(TimeSpan maxWaitTime, TimeSpan timeBetweenChecks, TimeSpan? releaseDeviceAfter = null)
        {
            _maxWaitTime = maxWaitTime;
            _timeBetweenChecks = timeBetweenChecks;
            _releaseDeviceAfter = releaseDeviceAfter;
        }

        /// <summary>
        /// Get an available device from the possible devices list
        /// </summary>
        /// <param name="possibleDevices">An array of possible devices</param>
        /// <returns>An available device to use</returns>
        public IAndroidDevice GetDevice(IList<DeviceConfiguration> possibleDevices)
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

        /// <summary>
        /// Get an available device from all connected devices (will look at the adb device list)
        /// </summary>
        /// <param name="adbPath">Absolute path to adb.exe</param>
        /// <param name="dependencyHandling">How to handle dependencies</param>
        /// <returns>An available device to use</returns>
        public IAndroidDevice GetDevice(string adbPath = null, DependencyHandling dependencyHandling = DependencyHandling.InstallIfMissing)
        {
            var terminal = new AdbCommandExecutor(adbPath: adbPath);
            var devices = terminal.ExecuteAdbCommand("devices").Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (devices.Length <= 1)
            {
                throw new AndroidDeviceFactoryException("Could not find any connected devices");
            }

            var possibleDevices = new List<DeviceConfiguration>();
            for (var i = 1; i < devices.Length; i++)
            {
                possibleDevices.Add(new DeviceConfiguration
                {
                    Serial = devices[i].Split('\t').First()
                });
            }

            var configuration = GetConfiguration(possibleDevices);
            return new AndroidDevice(new DeviceConfiguration
            {
                AdbPath = adbPath,
                Dependencies = dependencyHandling,
                Port = GetPort(possibleDevices, configuration),
                Serial = configuration.Serial
            });
        }

        /// <summary>
        /// Dispose device by stopping server and removing it from the busy list.
        /// </summary>
        /// <param name="device">Device to dispose</param>
        public void DisposeDevice(IAndroidDevice device)
        {
            lock (BusyDevices)
            {
                DeviceLogger.Log($"Disposing device {device.Serial}", DeviceLogger.LogLevel.Info);
                device.StopServer();
                if (BusyDevices.Any(b => b.configuration.Serial == device.Serial))
                {
                    BusyDevices.Remove(BusyDevices.First(b => b.configuration.Serial == device.Serial));
                }
            }
        }

        private DeviceConfiguration GetConfiguration(IList<DeviceConfiguration> possibleDevices)
        {
            var start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < _maxWaitTime.TotalMilliseconds)
            {
                lock (BusyDevices)
                {
                    ReleaseTimedOutDevices();

                    if (!possibleDevices.Any())
                    {
                        if (IsDeviceAvailable(string.Empty))
                        {
                            var configuration = new DeviceConfiguration();
                            BusyDevices.Add((configuration, DateTime.Now));
                            return configuration;
                        }
                    }

                    var availableDevice = possibleDevices.FirstOrDefault(p => IsDeviceAvailable(p.Serial));
                    if (availableDevice != null)
                    {
                        DeviceLogger.Log($" Device available: {availableDevice.Serial}", DeviceLogger.LogLevel.Info);
                        BusyDevices.Add((availableDevice, DateTime.Now));
                        return availableDevice;
                    }
                }

                Thread.Sleep(_timeBetweenChecks);
            }

            throw new AndroidDeviceFactoryException($" Could not find any available devices after {_maxWaitTime} minutes.");
        }

        private void ReleaseTimedOutDevices()
        {
            if (_releaseDeviceAfter == null)
            {
                return;
            }

            lock (BusyDevices)
            {
                var devices = BusyDevices
                    .Where(b => DateTime.Now > b.initialized.Add(_releaseDeviceAfter.Value))
                    .ToList();

                foreach (var device in devices)
                {
                    lock (BusyDevices)
                    {
                        DeviceLogger.Log($" Releasing device {device.configuration.Serial}", DeviceLogger.LogLevel.Info);
                        BusyDevices.Remove(device);
                    }
                }
            }
        }

        private bool IsDeviceAvailable(string serial)
        {
            return BusyDevices.All(b => b.configuration.Serial != serial);
        }

        private int GetPort(ICollection<DeviceConfiguration> possibleDevices, DeviceConfiguration configuration)
        {
            if (possibleDevices.Select(p => p.Port).Distinct().Count() == possibleDevices.Count)
            {
                return configuration.Port;
            }

            var stripedSerial = new string(configuration.Serial.Where(char.IsDigit).ToArray());
            return (int)(double.Parse(stripedSerial) % 65535);
        }
    }
}
