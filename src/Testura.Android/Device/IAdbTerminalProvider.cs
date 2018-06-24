using Testura.Android.Util;

namespace Testura.Android.Device
{
    /// <summary>
    /// Defines an inteface to provide an adb terminal
    /// </summary>
    public interface IAdbTerminalProvider
    {
        /// <summary>
        /// Gets a terminal object to send adb commands.
        /// </summary>
        AdbTerminal AdbTerminal { get; }
    }
}
