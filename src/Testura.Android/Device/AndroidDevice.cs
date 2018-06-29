using System.Linq;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Server;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Activity;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util;

namespace Testura.Android.Device
{
    /// <summary>
    /// Provides functionality to interact with an Android Device through multiple service objects
    /// </summary>
    public class AndroidDevice : IAndroidDevice, IAdbCommandExecutorProvider, IAndroidServiceProvider, IAndroidUiMapper
    {
        private readonly DeviceConfiguration _configuration;
        private readonly UiAutomatorServer _uiAutomatorServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        /// <param name="configuration">Device Configuration</param>
        public AndroidDevice(DeviceConfiguration configuration)
        {
            var adbCommandExecutor = new AdbCommandExecutor(configuration.Serial, configuration.AdbPath);

            _configuration = configuration;
            _uiAutomatorServer = new UiAutomatorServer(adbCommandExecutor, configuration.Port);

            Adb = new AdbService(adbCommandExecutor);
            Ui = new UiService(_uiAutomatorServer, new NodeParser(), new NodeFinder());
            Settings = new SettingsService(Adb);
            Activity = new ActivityService(Adb);
            Interaction = new InteractionService(Adb, _uiAutomatorServer);

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
        /// Gets the adb service of an android device. This service
        /// handles everything connected to adb.
        /// </summary>
        public AdbService Adb { get; }

        /// <summary>
        /// Gets the ui service of an android device.
        /// </summary>
        public UiService Ui { get; }

        /// <summary>
        /// Gets the settings service of an android device. This service
        /// handles everything with settings like airplane mode, wifi, etc.
        /// </summary>
        public SettingsService Settings { get; }

        /// <summary>
        /// Gets the activity service of an android device. This service
        /// handles everything connected to activites (start, current, etc).
        /// </summary>
        public ActivityService Activity { get; }

        /// <summary>
        /// Gets the interaction service of an android device. This service
        /// handles everything with interaction like swipe, tap, etc.
        /// </summary>
        public InteractionService Interaction { get; }

        /// <summary>
        /// Gets the serial of the device
        /// </summary>
        public string Serial => _configuration.Serial;

        /// <summary>
        /// Create a new ui object that maps to a single node
        /// </summary>
        /// <param name="by">How we should map ui object. Node should match all provided bys</param>
        /// <returns>The mapped ui object</returns>
        public virtual UiObject MapUiObject(params By[] by)
        {
            return new UiObject(Interaction, Ui, by);
        }

        /// <summary>
        /// Create a new ui object that maps to multiple nodes with same matching properties
        /// </summary>
        /// <param name="by">How we should map ui object. Node should match all provided bys</param>
        /// <returns>The mapped ui object</returns>
        public virtual UiObjects MapUiObjects(params By[] by)
        {
            return new UiObjects(Ui, by);
        }

        /// <summary>
        /// Start the ui server
        /// </summary>
        public void StartServer()
        {
            _uiAutomatorServer.Start();
        }

        /// <summary>
        /// Stop the ui server
        /// </summary>
        public void StopServer()
        {
            _uiAutomatorServer.Stop();
        }

        /// <summary>
        /// Get an adb terminal configured for this device.
        /// </summary>
        /// <returns>Adb terminal configured for this device</returns>
        public AdbCommandExecutor GetAdbCommandExecutor()
        {
            return new AdbCommandExecutor(_configuration.Serial, _configuration.AdbPath);
        }

        private void InstallHelperApks()
        {
            var dependencyInstaller = new DependencyInstaller();
            switch (_configuration.Dependencies)
            {
                case DependencyHandling.AlwaysInstall:
                    dependencyInstaller.InstallDependencies(Adb, _configuration.DependenciesDirectory);
                    break;
                case DependencyHandling.InstallIfMissing:
                    dependencyInstaller.InstallDependenciesIfMissing(Adb, Activity, _configuration.DependenciesDirectory);
                    break;
            }
        }
    }
}
