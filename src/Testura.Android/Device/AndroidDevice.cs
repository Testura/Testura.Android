using System;
using System.Collections.Generic;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Server;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Activity;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
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

        /// <inheritdoc />
        public AdbService Adb { get; }

        /// <inheritdoc />
        public UiService Ui { get; }

        /// <inheritdoc />
        public SettingsService Settings { get; }

        /// <inheritdoc />
        public ActivityService Activity { get; }

        /// <inheritdoc />
        public InteractionService Interaction { get; }

        /// <inheritdoc />
        public string Serial => _configuration.Serial;

        /// <inheritdoc />
        public virtual UiObject MapUiObject(Func<Node, string, bool> expression, string customErrorMessage = null)
        {
            return new UiObject(Interaction, Ui, new List<Where> { Where.Lambda(expression, customErrorMessage) }, null);
        }

        /// <inheritdoc />
        public virtual UiObject MapUiObject(Func<Node, bool> expression, string customErrorMessage = null)
        {
            return new UiObject(Interaction, Ui, new List<Where> { Where.Lambda(expression, customErrorMessage) }, null);
        }

        /// <inheritdoc />
        public UiObject MapUiObject(params Where[] wheres)
        {
            return new UiObject(Interaction, Ui, wheres, null);
        }

        /// <inheritdoc />
        public void StartServer()
        {
            _uiAutomatorServer.Start();
        }

        /// <inheritdoc />
        public void StopServer()
        {
            _uiAutomatorServer.Stop();
        }

        /// <inheritdoc />
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
