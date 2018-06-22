using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Activity;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Device.Services.Ui;

namespace Testura.Android.Device
{
    public interface IAndroidServiceContainer
    {
        /// <summary>
        /// Gets the activity service of an android device.
        /// </summary>
        ActivityService Activity { get; }

        /// <summary>
        /// Gets the adb service of an android device.
        /// </summary>
        AdbService Adb { get; }

        /// <summary>
        /// Gets the interaction service of an android device.
        /// </summary>
        InteractionService Interaction { get; }

        /// <summary>
        /// Gets the settings service of an android device.
        /// </summary>
        SettingsService Settings { get; }

        /// <summary>
        /// Gets the ui service of an android device.
        /// </summary>
        UiService Ui { get; }
    }
}
