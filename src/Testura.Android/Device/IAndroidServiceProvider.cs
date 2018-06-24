using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Activity;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Device.Services.Ui;

namespace Testura.Android.Device
{
    /// <summary>
    /// Defines an interface to provide android services.
    /// </summary>
    public interface IAndroidServiceProvider
    {
        /// <summary>
        /// Gets the activity service of an android device. This service
        /// handles everything connected to activites (start, current, etc).
        /// </summary>
        ActivityService Activity { get; }

        /// <summary>
        /// Gets the adb service of an android device. This service
        /// handles everything connected to adb.
        /// </summary>
        AdbService Adb { get; }

        /// <summary>
        /// Gets the interaction service of an android device. This service
        /// handles everything with interaction like swipe, tap, etc.
        /// </summary>
        InteractionService Interaction { get; }

        /// <summary>
        /// Gets the settings service of an android device. This service
        /// handles everything with settings like airplane mode, wifi, etc.
        /// </summary>
        SettingsService Settings { get; }

        /// <summary>
        /// Gets the ui service of an android device.
        /// </summary>
        UiService Ui { get; }
    }
}
