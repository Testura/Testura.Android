using System.Linq;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Activity;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Device.Ui.Server;
using Testura.Android.Util;

namespace Testura.Android.Device
{
    /// <summary>
    /// Provides functionality to interact with an Android Device through multiple service objects
    /// </summary>
    public class AndroidDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        /// <param name="configuration">Device Configuration</param>
        internal AndroidDevice(DeviceConfiguration configuration)
        {
            Configuration = configuration;
            var terminal = new Terminal(configuration.Serial, configuration.AdbPath);
            var server = new UiAutomatorServer(terminal, configuration.Port.Value, configuration.DumpTimeout);

            Adb = new AdbService(terminal);
            Ui = new UiService(new ScreenDumper(server, configuration.DumpTries), new NodeParser(), new NodeFinder());
            Settings = new SettingsService(Adb);
            Activity = new ActivityService(Adb);
            Interaction = new InteractionService(Adb, server);
            InstallHelperApks();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDevice"/> class.
        /// </summary>
        internal AndroidDevice()
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
        public AdbService Adb { get; }

        /// <summary>
        /// Gets the ui service of an android device.
        /// </summary>
        public UiService Ui { get; }

        /// <summary>
        /// Gets the settings service of an android device.
        /// </summary>
        public SettingsService Settings { get; }

        /// <summary>
        /// Gets the activity service of an android device.
        /// </summary>
        public ActivityService Activity { get; }

        /// <summary>
        /// Gets the interaction service of an android device.
        /// </summary>
        public InteractionService Interaction { get; }

        /// <summary>
        /// Create a new ui object that wraps around a node that match a specific search criteria
        /// </summary>
        /// <param name="with">Find node with</param>
        /// <returns>The mapped ui object</returns>
        public UiObject CreateUiObject(params With[] with)
        {
            return new UiObject(Interaction, Ui, with.ToArray());
        }

        /// <summary>
        /// Create a new ui object that wraps around multiple nodes that match a specific search criteria
        /// </summary>
        /// <param name="with">Find nodes with</param>
        /// <returns>The mapped ui object</returns>
        public UiObjects CreateUiObjects(params With[] with)
        {
            return new UiObjects(Ui, with.ToArray());
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
