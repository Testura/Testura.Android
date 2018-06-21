#pragma warning disable IDE0005 // Using directive is unnecessary.
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace Testura.Android.Device.Server
{
    /// <summary>
    /// Defines methods to interact with the UI automator server.
    /// </summary>
    public interface IUiAutomatorServer
    {
        /// <summary>
        /// Send a screen dump request to the ui automator server on the android device.
        /// </summary>
        /// <returns>The screen content as a xml string.</returns>
        string DumpUi();
    }
}