#pragma warning disable IDE0005 // Using directive is unnecessary.
using System.Collections;
using System.Collections.Generic;
using Testura.Android.Util.Exceptions;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace Testura.Android.Device.Services
{
    /// <summary>
    /// Defines methods to interact with activities on an android device.
    /// </summary>
    public interface IActivityService
    {
        /// <summary>
        /// Start an Activity specified by package and activity name.
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <param name="activity">Name of the activity.</param>
        /// <param name="forceStopActivity">Force stop the target app before starting the activity.</param>
        /// <param name="clearTasks">Clear activity stack.</param>
        /// <exception cref="AdbException">Thrown if we can't find package/activity.</exception>
        void Start(string packageName, string activity, bool forceStopActivity, bool clearTasks);

        /// <summary>
        /// Get the current open activity.
        /// </summary>
        /// <returns>Current open activity.</returns>
        string GetCurrent();

        /// <summary>
        /// Get a list with all installed packages on the android device.
        /// </summary>
        /// <returns>A list with all installed packages.</returns>
        IList<string> GetPackages();

        /// <summary>
        /// Examine if a specific package is installed.
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <returns>True if package are installed, otherwise false</returns>
        bool IsPackagedInstalled(string packageName);
    }
}
