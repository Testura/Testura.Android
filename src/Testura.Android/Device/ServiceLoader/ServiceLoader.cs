using Testura.Android.Device.Configurations;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Default;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Server;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Device.ServiceLoader
{
    /// <summary>
    /// Default implementation of the service loader
    /// </summary>
    public class ServiceLoader : IServiceLoader
    {
        /// <summary>
        /// Load the adb service
        /// </summary>
        /// <param name="configuration">Current device configuration</param>
        /// <returns>The loaded adb service</returns>
        public virtual IAdbService LoadAdbService(DeviceConfiguration configuration)
        {
            return new AdbService(new Terminal(configuration));
        }

        /// <summary>
        /// Load the activity service
        /// </summary>
        /// <param name="configuration">Current device configuration</param>
        /// <returns>The loaded activity service</returns>
        public virtual IActivityService LoadActivityService(DeviceConfiguration configuration)
        {
            return new ActivityService();
        }

        /// <summary>
        /// Load the settings service
        /// </summary>
        /// <param name="configuration">Current device configuration</param>
        /// <returns>The loaded settings service</returns>
        public virtual ISettingsService LoadSettingsService(DeviceConfiguration configuration)
        {
            return new SettingsService();
        }

        /// <summary>
        /// Load the ui service
        /// </summary>
        /// <param name="configuration">Current device configuration</param>
        /// <returns>The loaded ui service</returns>
        public virtual IUiService LoadUiService(DeviceConfiguration configuration)
        {
            return new UiService(
                new ScreenDumper(new UiAutomatorServer(new Terminal(configuration), configuration.Port)),
                new NodeParser(),
                new NodeFinder());
        }

        /// <summary>
        /// Load the interaction service
        /// </summary>
        /// <param name="configuration">Current device configuration</param>
        /// <returns>The loaded interaction service</returns>
        public virtual IInteractionService LoadInteractionService(DeviceConfiguration configuration)
        {
            return new InteractionService();
        }
    }
}
