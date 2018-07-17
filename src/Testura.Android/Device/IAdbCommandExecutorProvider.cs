using Testura.Android.Util;

namespace Testura.Android.Device
{
    /// <summary>
    /// Defines an inteface to provide an adb terminal
    /// </summary>
    public interface IAdbCommandExecutorProvider
    {
        /// <summary>
        /// Get an adb terminal configured for this device.
        /// </summary>
        /// <returns>Adb terminal configured for this device</returns>
        AdbCommandExecutor GetAdbCommandExecutor();
    }
}
