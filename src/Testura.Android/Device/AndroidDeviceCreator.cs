namespace Testura.Android.Device
{
    /*
    public static class AndroidDeviceCreator
    {
        private static IList<AndroidDevice> _busyDevices = new List<AndroidDevice>();

        public static AndroidDevice Create(IList<DeviceConfiguration> possibleDevices, TimeSpan timeout)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMinutes < timeout.TotalMinutes)
            {
                lock (_busyDevices)
                {
                    var availableDevice = possibleDevices.Any(p => !_busyDevices.Any(b => b.s))
                }
            }
        }

        public static void DisposeDevice(AndroidDevice device)
        {
            lock (_busyDevices)
            {
                _busyDevices.Remove(device);
            }
        }

        public static void DisposeAllDevices()
        {
            lock (_busyDevices)
            {
                foreach (var androidDevice in _busyDevices)
                {
                    androidDevice.Ui.StopUiServer();
                }

                _busyDevices.Clear();
            }
        }
    }
    */
}
