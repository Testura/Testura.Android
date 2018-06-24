namespace Testura.Android.Device
{
    /// <summary>
    /// Defines an interface to control the android device.
    /// </summary>
    public interface IAndroidDevice : IAndroidUiMapper, IAndroidServiceProvider, IAdbTerminalProvider
    {
        /// <summary>
        /// Gets the serial of the device
        /// </summary>
        string Serial { get; }

        /// <summary>
        /// Start the ui server
        /// </summary>
        void StartServer();

        /// <summary>
        /// Stop the ui server
        /// </summary>
        void StopServer();
    }
}