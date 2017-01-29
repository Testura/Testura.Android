#pragma warning disable IDE0005 // Using directive is unnecessary.
using Testura.Android.Util.Exceptions;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace Testura.Android.Device.Services
{
    public interface IActivityService
    {
        /// <summary>
        /// Start an Activity specified by package and activity name
        /// </summary>
        /// <param name="packageName">Name of the package</param>
        /// <param name="activity">Name of the activity</param>
        /// <param name="forceStopActivity">Force stop the target app before starting the activity.</param>
        /// <param name="clearTasks">Clear activity stack</param>
        /// <exception cref="AdbException">Thrown if we can't find package/activity</exception>
        void Start(string packageName, string activity, bool forceStopActivity, bool clearTasks);

        /// <summary>
        /// Get the current open activity
        /// </summary>
        /// <returns>Current open activity</returns>
        string GetCurrent();
    }
}
