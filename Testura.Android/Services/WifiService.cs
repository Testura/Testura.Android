using Testura.Android.Device;
using Testura.Android.Util;

namespace Testura.Android.Services
{
    public class WifiService : Service, IWifiService
    {
        public WifiService(IDeviceServices deviceServices, ITerminal terminal)
            : base(deviceServices)
        {
        }

        public WifiService()
        {
        }

        public void EnableWifi()
        {
            DeviceServices.Adb.Shell(
                "am startservice -n com.testura.testuraandroidserver/.services.settings.WifiService -e enable 1");
        }

        public void DisableWifi()
        {
            DeviceServices.Adb.Shell(
                "am startservice -n com.testura.testuraandroidserver/.services.settings.WifiService -e enable 0");
        }
    }
}
