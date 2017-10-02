using System.Linq;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Default;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Server;
using Testura.Android.Util;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Device
{
    /// <summary>
    /// Provides functionality to interact with an Android Device through multiple service objects
    /// </summary>
    public class AndroidDevice : IAndroidDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        /// <param name="configuration">Device Configuration</param>
        /// <param name="adbService">Service to handle communication with adb</param>
        /// <param name="uiService">Service to handle UI</param>
        /// <param name="settingsService">Service to handle settings</param>
        /// <param name="activityService">Service to handle activities</param>
        /// <param name="interactionService">Service to handle interaction with the device</param>
        public AndroidDevice(
            DeviceConfiguration configuration,
            IAdbService adbService,
            IUiService uiService,
            ISettingsService settingsService,
            IActivityService activityService,
            IInteractionService interactionService)
        {
            Configuration = configuration;
            Adb = adbService;
            Ui = uiService;
            Settings = settingsService;
            Activity = activityService;
            Interaction = interactionService;
            SetOwner();
            InstallHelperApks();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        /// <param name="configuration">Device Configuration</param>
        public AndroidDevice(DeviceConfiguration configuration)
        {
            Configuration = configuration;
            var server = new UiAutomatorServer(new Terminal(configuration), configuration.Port);
            Adb = new AdbService(new Terminal(configuration));
            Ui = new UiService(
                new ScreenDumper(server, configuration.DumpTries),
                new NodeParser(),
                new NodeFinder());
            Settings = new SettingsService();
            Activity = new ActivityService();
            Interaction = new InteractionService(server);
            SetOwner();
            InstallHelperApks();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        public AndroidDevice()
            : this(new DeviceConfiguration())
        {
        }

        /// <summary>
        /// Gets the current device Configuration.
        /// </summary>
        public DeviceConfiguration Configuration { get; }

        /// <summary>
        /// Gets the adb service of an android device.
        /// </summary>
        public IAdbService Adb { get; }

        /// <summary>
        /// Gets the ui service of an android device.
        /// </summary>
        public IUiService Ui { get; }

        /// <summary>
        /// Gets the settings service of an android device.
        /// </summary>
        public ISettingsService Settings { get; }

        /// <summary>
        /// Gets the activity service of an android device.
        /// </summary>
        public IActivityService Activity { get; }

        /// <summary>
        /// Gets the interaction service of an android device.
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
            var dependencyInstaller = new DependencyInstaller();
            if (Configuration.Dependencies == DependencyHandling.AlwaysInstall)
            {
                dependencyInstaller.InstallDependencies(Adb, Configuration);
            }
            else if (Configuration.Dependencies == DependencyHandling.InstallIfMissing)
            {
                dependencyInstaller.InstallDependenciesIfMissing(Adb, Activity, Configuration);
            }
        }
    }
}
