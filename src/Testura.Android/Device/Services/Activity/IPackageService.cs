using System;
using System.Collections.Generic;

namespace Testura.Android.Device.Services.Activity
{
    public interface IPackageService
    {
        /// <summary>
        /// Get a list with all installed packages on the android device
        /// </summary>
        /// <returns>A list with all installed packages</returns>
        IList<string> GetPackages();

        /// <summary>
        /// Check if a specific package is installed
        /// </summary>
        /// <param name="packageName">Name of the package</param>
        /// <returns>True if package are installed, otherwise false</returns>
        bool IsPackagedInstalled(string packageName);

        /// <summary>
        /// Get the package name.
        /// </summary>
        /// <param name="packageName">Name of package.</param>
        /// <returns>The package version if package exist, otherwise version 0.</returns>
        Version GetPackageVersion(string packageName);
    }
}
