#pragma warning disable IDE0005 // Using directive is unnecessary.
using Testura.Android.Util.Exceptions;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace Testura.Android.Device.Ui.Server
{
    public interface IUiAutomatorServer
    {
        /// <summary>
        /// Start the ui automator server on the android device
        /// </summary>
        /// <exception cref="UiAutomatorServerException">Thrown if we can't server</exception>
        void Start();

        /// <summary>
        /// Stop the ui automator server on the android device
        /// </summary>
        void Stop();

        /// <summary>
        /// Check if the ui automator server is alive on the android device
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>True if server is a alive, false otherwise.</returns>
        bool Alive(int timeout);

        /// <summary>
        /// Send a screen dump request to the ui automator server on the android device.
        /// </summary>
        /// <returns>The screen content as a xml string</returns>
        string DumpUi();
    }
}