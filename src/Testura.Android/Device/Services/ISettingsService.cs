using Testura.Android.Util;

namespace Testura.Android.Device.Services
{
    /// <summary>
    /// Defines methods to change settings on android device.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Enable or disable wifi.
        /// </summary>
        /// <param name="state">Wanted state of wifi.</param>
        void Wifi(State state);

        /// <summary>
        /// Enable or disable gps.
        /// </summary>
        /// <param name="state">Wanted state of gps.</param>
        void Gps(State state);

        /// <summary>
        /// Enable or disable airplane mode.
        /// </summary>
        /// <param name="state">Wanted state of airplane mode.</param>
        void AirplaneMode(State state);
    }
}