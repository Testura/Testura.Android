using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Device.Services.Default
{
    /// <summary>
    /// Provides functionality to interact with activities on an android device
    /// </summary>
    public class ActivityService : Service, IActivityService
    {
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

            DeviceLogger.Log("Starting a new activity");
            var commandBuilder = new StringBuilder($"am start -W -n {packageName}/{activity}");
            if (forceStopActivity)
            {
                commandBuilder.Append(" -S");
            }

            if (clearTasks)
            {
                commandBuilder.Append(" --activity-clear-task");
            }

            var result = Device.Adb.Shell(commandBuilder.ToString());
            if (result.Contains("Error") || result.Contains("does not exist") || result.Contains("Exception"))
            {
                throw new AdbException(result);
            }

            Device.Ui.ClearCache();
        }

        /// <summary>
        /// Get the current open activity
        /// </summary>
        /// <returns>Current open activity</returns>
        public string GetCurrent()
        {
            DeviceLogger.Log("Getting current activity");
            var activity = Device.Adb.Shell("dumpsys window windows | grep -E 'mCurrentFocus|mFocusedApp'");
            var regex = new Regex(@"(?<=\{)[^}]*(?=\})");
            var matches = regex.Matches(activity);
            if (matches.Count == 0)
            {
                return "Unknown activity";
            }

            return matches[0].Value.Split(' ').Last();
        }

        /// <summary>
        /// Get a list with all installed packages on the android device
        /// </summary>
        /// <returns>A list with all installed packages</returns>
        public IList<string> GetPackages()
        {
            var packages = Device.Adb.Shell("pm list packages").Split(new[] { "\r\r\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return packages.Select(p => p.Replace("package:", string.Empty)).ToList();
        }

        /// <summary>
        /// Check if a specific package is installed
        /// </summary>
        /// <param name="packageName">Name of the package</param>
        /// <returns>True if package are installed, otherwise false</returns>
        public bool IsPackagedInstalled(string packageName)
        {
            var packages = GetPackages();
            return packages.Contains(packageName);
        }

        /// <summary>
        /// Get the package name.
        /// </summary>
        /// <param name="packageName">Name of package.</param>
        /// <returns>The package version if package exist, otherwise version 0.</returns>
        public Version GetPackageVersion(string packageName)
        {
            var version = Device.Adb.Shell($"dumpsys package {packageName} | grep versionName");
            var versionSplit = version
                .Trim()
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Split('=');

            if (versionSplit.Length != 2)
            {
                return new Version(0, 0);
            }

            return Version.Parse(versionSplit[1]);
        }
    }
}
