﻿using System.IO;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Services;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util
{
    internal class DependencyInstaller
    {
        public void InstallDependencies(IAdbService adbService, DeviceConfiguration configuration)
        {
            DeviceLogger.Log("Installing all dependencies..");
            adbService.InstallApp(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.HelperApkName));
            adbService.InstallApp(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.ServerApkName));
        }

        public void InstallDependenciesIfMissing(IAdbService adbService, IActivityService activityService, DeviceConfiguration configuration)
        {
            DeviceLogger.Log("Checking if helper is installed..");
            if (!activityService.IsPackagedInstalled("com.testura.helper"))
            {
                DeviceLogger.Log("..not installed, installing..");
                adbService.InstallApp(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.HelperApkName));
            }
            else
            {
                DeviceLogger.Log("..already installed.");
            }

            DeviceLogger.Log("Checking if server is installed..");
            if (!activityService.IsPackagedInstalled("com.testura.helper.test"))
            {
                DeviceLogger.Log("..not installed, installing..");
                adbService.InstallApp(Path.Combine(configuration.DependenciesDirectory, DeviceConfiguration.ServerApkName));
            }
            else
            {
                DeviceLogger.Log("..already installed.");
            }
        }
    }
}