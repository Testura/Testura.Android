﻿using Testura.Android.Device.Configurations;
using Testura.Android.Device.Services;
using Testura.Android.Util.Logging;

namespace Testura.Android.Device
{
    public interface IAndroidDevice
    {
        /// <summary>
        /// Gets the current device Configuration
        /// </summary>
        DeviceConfiguration Configuration { get; }

        /// <summary>
        /// Gets the adb service of an android device
        /// </summary>
        IAdbService Adb { get; }

        /// <summary>
        /// Gets the ui service of an android device
        /// </summary>
        IUiService Ui { get; }

        /// <summary>
        /// Gets the settings service of an andoid device
        /// </summary>
        ISettingsService Settings { get;  }

        /// <summary>
        /// Gets the activity service of an android device
        /// </summary>
        IActivityService Activity { get; }

        /// <summary>
        /// Gets the interaction service of an android device
        /// </summary>
        IInteractionService Interaction { get; }

        /// <summary>
        /// Add new log listeners
        /// </summary>
        /// <param name="logListener">Log listener to add</param>
        void AddLogListener(ILogListener logListener);
    }
}
