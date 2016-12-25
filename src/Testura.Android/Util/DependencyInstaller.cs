using System.IO;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Services;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util
{
    internal class DependencyInstaller
    {
        public void InstallDependencies(IAdbService adbService, DeviceConfiguration configuration)
        {
            DeviceLogger.Log("Installing dependencies..");
            adbService.InstallApp(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.HelperApkName));
            adbService.Push(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.UiAutomatorStub), @"/data/local/tmp/");
            adbService.Push(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.UiAutomatorStubBundle), @"/data/local/tmp/");
        }
    }
}
