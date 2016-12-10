using Testura.Android.Util;

namespace Testura.Android.Device.Services.Default
{
    public class SettingsService : Service, ISettingsService
    {
        /// <summary>
        /// Enable or disable wifi
        /// </summary>
        /// <param name="state">Wanted state of wifi</param>
        public void Wifi(State state)
        {
            Device.Adb.Shell(state == State.Enable
                ? "am startservice -n com.testura.testuraandroidserver/.services.settings.WifiService -e enable 1"
                : "am startservice -n com.testura.testuraandroidserver/.services.settings.WifiService -e enable 0");
        }

        /// <summary>
        /// Enable or disable airplane mode
        /// </summary>
        /// <param name="state">Wanted state of airplane mode</param>
        public void AirplaneMode(State state)
        {
            Device.Adb.Shell(state == State.Enable
                ? "settings put global airplane_mode_on 1"
                : "settings put global airplane_mode_on 0");

            Device.Adb.Shell("am broadcast -a android.intent.action.AIRPLANE_MODE");
        }
    }
}
