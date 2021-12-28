﻿using System.Reflection;
using Testura.Android.Device.Services.Activity;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util
{
    internal class DependencyInstaller
    {
        /// <summary>
        /// Name of the Testura helper APK file.
        /// </summary>
        public const string ServerApkName = "Testura.Android.Server.apk";

        /// <summary>
        /// Name of the Testura server APK file.
        /// </summary>
        public const string ServerUiAutomatorApkName = "Testura.Android.Server-UiAutomator.apk";

        /// <summary>
        /// Latest Testura server apk version
        /// </summary>
        public const string ServerApkVersion = "1.1";

        public void InstallDependencies(IAdbInstallService adbInstallService, string dependenciesDirectory)
        {
            CheckForApks();
            DeviceLogger.Log("Installing all dependencies..", DeviceLogger.LogLevel.Info);
            adbInstallService.InstallApp(Path.Combine(dependenciesDirectory, ServerApkName));
            adbInstallService.InstallApp(Path.Combine(dependenciesDirectory, ServerUiAutomatorApkName));
        }

        public void InstallDependenciesIfMissing(IAdbInstallService adbInstallService, IPackageService activityService, string dependenciesDirectory)
        {
            DeviceLogger.Log("Checking if helper is installed..", DeviceLogger.LogLevel.Info);
            if (!activityService.IsPackagedInstalled("com.testura.server"))
            {
                DeviceLogger.Log("..not installed.", DeviceLogger.LogLevel.Info);
                InstallDependencies(adbInstallService, dependenciesDirectory);
            }
            else
            {
                DeviceLogger.Log("..already installed.", DeviceLogger.LogLevel.Info);
                var latestVersion = Version.Parse(ServerApkVersion);
                if (activityService.GetPackageVersion("com.testura.server") < latestVersion)
                {
                    DeviceLogger.Log("But you don't have the current/latest version. Updating your dependencies", DeviceLogger.LogLevel.Info);
                    InstallDependencies(adbInstallService, dependenciesDirectory);
                }
            }
        }

        private void CheckForApks()
        {
            DeviceLogger.Log("Looking for APKs..", DeviceLogger.LogLevel.Info);

            if (!File.Exists(ServerApkName))
            {
                DeviceLogger.Log($"Could not find {ServerApkName}, extracting from dll.", DeviceLogger.LogLevel.Info);
                ExtractApkFromDll(ServerApkName);
            }

            if (!File.Exists(ServerUiAutomatorApkName))
            {
                DeviceLogger.Log($"Could not find {ServerUiAutomatorApkName}, extracting from dll.", DeviceLogger.LogLevel.Info);
                ExtractApkFromDll(ServerUiAutomatorApkName);
            }

            DeviceLogger.Log("..APKs OK", DeviceLogger.LogLevel.Info);
        }

        private void ExtractApkFromDll(string name)
        {
            using var resource = typeof(DependencyInstaller).GetTypeInfo().Assembly.GetManifestResourceStream(name);
            using var file = new FileStream(name, FileMode.Create, FileAccess.Write);
            resource.CopyTo(file);
        }
    }
}
