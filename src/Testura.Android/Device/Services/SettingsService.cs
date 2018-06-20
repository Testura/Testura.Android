using Testura.Android.Device.Services.Adb;
using Testura.Android.Util;
using Testura.Android.Util.Logging;

namespace Testura.Android.Device.Services
{
    /// <summary>
    /// Provides functionality to change settings on android device.
    /// </summary>
    public class SettingsService
    {
        private readonly IAdbShellService _adbShellService;
        private const string PackageName = "com.testura.server";

        public SettingsService(IAdbShellService adbShellService)
        {
            _adbShellService = adbShellService;
        }

        /// <summary>
        /// Enable or disable wifi
        /// </summary>
        /// <param name="state">Wanted state of wifi</param>
        public void Wifi(State state)
        {
            DeviceLogger.Log("Changing wifi state");
            _adbShellService.Shell(state == State.Enable
                ? $"am startservice -n {PackageName}/{PackageName}.services.settings.WifiService -e enable 1"
                : $"am startservice -n {PackageName}/{PackageName}.services.settings.WifiService  -e enable 0");
        }

        /// <summary>
        /// Enable or disable bluetooth.
        /// </summary>
        /// <param name="state">Wanted state of bluetooth.</param>
        public void Bluetooth(State state)
        {
            DeviceLogger.Log("Changing wifi state");
            _adbShellService.Shell(state == State.Enable
                ? $"am startservice -n {PackageName}/{PackageName}.services.settings.BluetoothService -e enable 1"
                : $"am startservice -n {PackageName}/{PackageName}.services.settings.BluetoothService  -e enable 0");
        }

        /// <summary>
        /// Enable or disable gps
        /// </summary>
        /// <param name="state">Wanted state of gps</param>
        public void Gps(State state)
        {
            DeviceLogger.Log("Changing gps state");
            if (state == State.Enable)
            {
                _adbShellService.Shell("settings put secure location_providers_allowed +gps");
                _adbShellService.Shell("settings put secure location_providers_allowed +network");
                _adbShellService.Shell("settings put secure location_providers_allowed +wifi");
            }
            else
            {
                _adbShellService.Shell("settings put secure location_providers_allowed -gps");
                _adbShellService.Shell("settings put secure location_providers_allowed -network");
                _adbShellService.Shell("settings put secure location_providers_allowed -wifi");
            }
        }

        /// <summary>
        /// Enable or disable airplane mode
        /// </summary>
        /// <param name="state">Wanted state of airplane mode</param>
        public void AirplaneMode(State state)
        {
            DeviceLogger.Log("Changing airplane mode state");
            _adbShellService.Shell(state == State.Enable
                ? "settings put global airplane_mode_on 1"
                : "settings put global airplane_mode_on 0");

            _adbShellService.Shell("am broadcast -a android.intent.action.AIRPLANE_MODE");
        }
    }
}
