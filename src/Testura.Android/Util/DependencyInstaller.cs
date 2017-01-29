using System.IO;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Services;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util
{
    internal class DependencyInstaller
    {
        private const string DevicePath = @"/data/local/tmp/";

        public void InstallDependencies(IAdbService adbService, DeviceConfiguration configuration)
        {
            DeviceLogger.Log("Installing dependencies..");
            adbService.InstallApp(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.HelperApkName));
            adbService.Push(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.UiAutomatorStub), DevicePath);
            adbService.Push(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.UiAutomatorStubBundle), DevicePath);
        }

        public void InstallDependenciesIfMissing(IAdbService adbService, IActivityService activityService, DeviceConfiguration configuration)
        {
            if (!activityService.IsPackagedInstalled("com.testura.helper"))
            {
                adbService.InstallApp(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.HelperApkName));
            }

            var files = adbService.Shell($"ls {DevicePath}");
            if (!files.Contains(DeviceConfiguration.UiAutomatorStub) ||
                !files.Contains(DeviceConfiguration.UiAutomatorStubBundle))
            {
                adbService.Push(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.UiAutomatorStub), DevicePath);
                adbService.Push(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.UiAutomatorStubBundle), DevicePath);
            }
        }
    }
}
