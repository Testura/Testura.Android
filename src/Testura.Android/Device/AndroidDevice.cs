using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.ServiceLoader;
using Testura.Android.Device.Services;
using Testura.Android.Util.Logging;

namespace Testura.Android.Device
{
    public class AndroidDevice : IAndroidDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        /// <param name="configuration">Device Configuration</param>
        /// <param name="serviceLoader">A custom service loader</param>
        public AndroidDevice(DeviceConfiguration configuration, IServiceLoader serviceLoader)
        {
            Configuration = configuration;
            Adb = serviceLoader.LoadAdbService(Configuration);
            Ui = serviceLoader.LoadUiService(Configuration);
            Settings = serviceLoader.LoadSettingsService(Configuration);
            Activity = serviceLoader.LoadActivityService(Configuration);
            Interaction = serviceLoader.LoadInteractionService(Configuration);
            SetOwner();
            InstallHelperApks();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        /// <param name="configuration">Device Configuration</param>
        public AndroidDevice(DeviceConfiguration configuration)
            : this(configuration, new ServiceLoader.ServiceLoader())
        {
        }

        /// <summary>
        /// Gets the current device Configuration
        /// </summary>
        public DeviceConfiguration Configuration { get; }

        /// <summary>
        /// Gets the adb service of an android device
        /// </summary>
        public IAdbService Adb { get; }

        /// <summary>
        /// Gets the ui service of an android device
        /// </summary>
        public IUiService Ui { get; }

        /// <summary>
        /// Gets the settings service of an andoid device
        /// </summary>
        public ISettingsService Settings { get; }

        /// <summary>
        /// Gets the activity service of an android device
        /// </summary>
        public IActivityService Activity { get; }

        /// <summary>
        /// Gets the interaction service of an android device
        /// </summary>
        public IInteractionService Interaction { get; }

        public void AddLogListener(ILogListener logListener)
        {
            TesturaLogger.AddListener(logListener);
        }

        private void SetOwner()
        {
            var components = GetType().GetProperties().Where(p => p.PropertyType.IsInterface);
            foreach (var component in components)
            {
                ((Service)component.GetValue(this)).InitializeServiceOwner(this);
            }
        }

        private void InstallHelperApks()
        {
            if (Configuration.ShouldInstallApk)
            {
                Adb.InstallApp(Configuration.ServerApkPath);
                Adb.InstallApp(Configuration.HelperApkPath);
            }
        }

        public void fgfdg()
        {
            TesturaLogger.Log("mm");
        }
    }
}
