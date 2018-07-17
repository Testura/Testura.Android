using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Device.Services.Activity
{
    /// <summary>
    /// Provides functionality to interact with activities on an android device
    /// </summary>
    public class ActivityService : IPackageService
    {
        private readonly IAdbShellService _adbShellService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityService"/> class.
        /// </summary>
        /// <param name="adbShellService">Service to execute shell command for specific device</param>
        public ActivityService(IAdbShellService adbShellService)
        {
            _adbShellService = adbShellService;
        }

        /// <summary>
        /// Start an Activity specified by package and activity name
        /// </summary>
        /// <param name="packageName">Name of the package</param>
        /// <param name="activity">Name of the activity</param>
        /// <param name="forceStopActivity">Force stop the target application before starting the activity.</param>
        /// <param name="clearTasks">Clear activity stack</param>
        /// <exception cref="AdbException">Thrown if we can't find package/activity</exception>
        public void Start(string packageName, string activity, bool forceStopActivity, bool clearTasks)
        {
            if (string.IsNullOrEmpty(packageName))
            {
                throw new ArgumentException("Argument is null or empty", nameof(packageName));
            }

            if (string.IsNullOrWhiteSpace(activity))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(activity));
            }

            DeviceLogger.Log("Starting a new activity", DeviceLogger.LogLevel.Info);
            var commandBuilder = new StringBuilder($"am start -W -n {packageName}/{activity}");
            if (forceStopActivity)
            {
                commandBuilder.Append(" -S");
            }

            if (clearTasks)
            {
                commandBuilder.Append(" --activity-clear-task");
            }

            var result = _adbShellService.Shell(commandBuilder.ToString());
            if (result.Contains("Error") || result.Contains("does not exist") || result.Contains("Exception"))
            {
                throw new AdbException(result);
            }
        }

        /// <summary>
        /// Get the current open activity
        /// </summary>
        /// <returns>Current open activity</returns>
        public string GetCurrent()
        {
            DeviceLogger.Log("Getting current activity", DeviceLogger.LogLevel.Info);
            var activity = _adbShellService.Shell("dumpsys window windows | grep -E 'mCurrentFocus|mFocusedApp'");
            var regex = new Regex(@"(?<=\{)[^}]*(?=\})");
            var matches = regex.Matches(activity);
            if (matches.Count == 0)
            {
                DeviceLogger.Log("Could not find any activity", DeviceLogger.LogLevel.Info);
                return "Unknown activity";
            }

            var foundActivity = matches[0].Value.Split(' ').Last();
            DeviceLogger.Log($"Found activity: {activity}", DeviceLogger.LogLevel.Info);
            return foundActivity;
        }

        /// <summary>
        /// Get a list with all installed packages on the android device
        /// </summary>
        /// <returns>A list with all installed packages</returns>
        public IList<string> GetPackages()
        {
            DeviceLogger.Log("Getting all packages", DeviceLogger.LogLevel.Info);
            var packages = _adbShellService.Shell("pm list packages").Split(new[] { "\r\r\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return packages.Select(p => p.Replace("package:", string.Empty)).ToList();
        }

        /// <summary>
        /// Check if a specific package is installed
        /// </summary>
        /// <param name="packageName">Name of the package</param>
        /// <returns>True if package are installed, otherwise false</returns>
        public bool IsPackagedInstalled(string packageName)
        {
            DeviceLogger.Log($"Checking if \"{packageName}\" package is installed..", DeviceLogger.LogLevel.Info);
            var packages = GetPackages();
            if (packages.Contains(packageName))
            {
                DeviceLogger.Log("..package is installed", DeviceLogger.LogLevel.Info);
                return true;
            }

            DeviceLogger.Log("..package is not installed", DeviceLogger.LogLevel.Info);
            return false;
        }

        /// <summary>
        /// Get the package name.
        /// </summary>
        /// <param name="packageName">Name of package.</param>
        /// <returns>The package version if package exist, otherwise version 0.</returns>
        public Version GetPackageVersion(string packageName)
        {
            DeviceLogger.Log($"Checking version of \"{packageName}\"...", DeviceLogger.LogLevel.Info);
            var version = _adbShellService.Shell($"dumpsys package {packageName} | grep versionName");
            var versionSplit = version
                .Trim()
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Split('=');

            if (versionSplit.Length != 2)
            {
                DeviceLogger.Log("...could not find any version", DeviceLogger.LogLevel.Info);
                return new Version(0, 0);
            }

            var parsedVersion = Version.Parse(versionSplit[1]);
            DeviceLogger.Log($"...version is {parsedVersion}", DeviceLogger.LogLevel.Info);
            return parsedVersion;
        }
    }
}
