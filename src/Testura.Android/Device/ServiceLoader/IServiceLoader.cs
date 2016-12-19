using Testura.Android.Device.Configurations;
using Testura.Android.Device.Services;

namespace Testura.Android.Device.ServiceLoader
{
    public interface IServiceLoader
    {
        /// <summary>
        /// Load the adb service
        /// </summary>
        /// <param name="configuration">Current device configuration</param>
        /// <returns>The loaded adb service</returns>
        IAdbService LoadAdbService(DeviceConfiguration configuration);

        /// <summary>
        /// Load the activity service
        /// </summary>
        /// <param name="deviceConfiguration">Current device Configuration</param>
        /// <returns>The loaded activity service</returns>
        IActivityService LoadActivityService(DeviceConfiguration deviceConfiguration);

        /// <summary>
        /// Load the settings service
        /// </summary>
        /// <param name="configuration">Current device configuration</param>
        /// <returns>The loaded settings service</returns>
        ISettingsService LoadSettingsService(DeviceConfiguration configuration);

        /// <summary>
        /// Load the ui service
        /// </summary>
        /// <param name="configuration">Current device configuration</param>
        /// <returns>The loaded ui service</returns>
        IUiService LoadUiService(DeviceConfiguration configuration);

        /// <summary>
        /// Load the interaction service
        /// </summary>
        /// <param name="configuration">Current device configuration</param>
        /// <returns>The loaded interaction service</returns>
        IInteractionService LoadInteractionService(DeviceConfiguration configuration);
    }
}
