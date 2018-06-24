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
    public class AndroidDevice : IAndroidDevice, IAdbTerminalProvider, IAndroidServiceProvider, IAndroidUiMapper
    {
        private readonly DeviceConfiguration _configuration;
        private readonly UiAutomatorServer _uiAutomatorServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        /// <param name="configuration">Device Configuration</param>
        public AndroidDevice(DeviceConfiguration configuration)
        {
            AdbTerminal = new AdbTerminal(configuration.Serial, configuration.AdbPath);

            _configuration = configuration;
            _uiAutomatorServer = new UiAutomatorServer(AdbTerminal, configuration.Port);

            Adb = new AdbService(AdbTerminal);
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
        /// Gets a terminal object to send adb commands.
        /// </summary>
        public AdbTerminal AdbTerminal { get; }


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
        /// Create a new ui object that wraps around a node that match a specific search criteria
        /// </summary>
        /// <param name="with">Find node with</param>
        /// <returns>The mapped ui object</returns>
        public virtual UiObject MapUiNode(params With[] with)
        {
            return new UiObject(Interaction, Ui, with.ToArray());
        }

        /// <summary>
        /// Create a new ui object that wraps around multiple nodes that match a specific search criteria
        /// </summary>
        /// <param name="with">Find nodes with</param>
        /// <returns>The mapped ui object</returns>
        public virtual UiObjects MapUiNodes(params With[] with)
        {
            return new UiObjects(Ui, with.ToArray());
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

        private void InstallHelperApks()
        {
            var dependencyInstaller = new DependencyInstaller();
            if (_configuration.Dependencies == DependencyHandling.AlwaysInstall)
            {
                dependencyInstaller.InstallDependencies(Adb, _configuration.DependenciesDirectory);
            }
            else if (_configuration.Dependencies == DependencyHandling.InstallIfMissing)
            {
                dependencyInstaller.InstallDependenciesIfMissing(Adb, Activity, _configuration.DependenciesDirectory);
            }
        }
    }
}
