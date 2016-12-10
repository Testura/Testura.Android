using System.Linq;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.ServiceLoader;
using Testura.Android.Device.Services;

namespace Testura.Android.Device
{
    public class AndroidDevice : IAndroidDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        /// <param name="deviceConfiguration">Device DeviceConfiguration</param>
        /// <param name="serviceLoader">A custom service loader</param>
        public AndroidDevice(DeviceConfiguration deviceConfiguration, IServiceLoader serviceLoader)
        {
            DeviceConfiguration = deviceConfiguration;
            Adb = serviceLoader.LoadAdbService(DeviceConfiguration);
            Ui = serviceLoader.LoadUiService(DeviceConfiguration);
            Settings = serviceLoader.LoadSettingsService(DeviceConfiguration);
            Activity = serviceLoader.LoadActivityService(DeviceConfiguration);
            Interaction = serviceLoader.LoadInteractionService(DeviceConfiguration);
            SetOwner();
            InstallHelperApks();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        /// <param name="deviceConfiguration">Device DeviceConfiguration</param>
        public AndroidDevice(DeviceConfiguration deviceConfiguration)
            : this(deviceConfiguration, new ServiceLoader.ServiceLoader())
        {
        }

        /// <summary>
        /// Gets the current device DeviceConfiguration
        /// </summary>
        public DeviceConfiguration DeviceConfiguration { get; }

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
            if (DeviceConfiguration.ShouldInstallApk)
            {
                Adb.InstallApp(DeviceConfiguration.ServerApkPath);
                Adb.InstallApp(DeviceConfiguration.HelperApkPath);
            }
        }
    }
}
