using System;
using System.IO;
using System.Reflection;
using Testura.Android.Util;
using Testura.Android.Util.Extensions;

namespace Testura.Android.Device.Configurations
{
    /// <summary>
    /// Represents the device configuration.
    /// </summary>
    public class DeviceConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceConfiguration"/> class.
        /// </summary>
        public DeviceConfiguration()
        {
            AdbPath = string.Empty;
            Serial = string.Empty;
            Dependencies = DependencyHandling.InstallIfMissing;
            DependenciesDirectory = Path.Combine(Assembly.GetExecutingAssembly().GetDirectoryPath(), "Dependencies");
            Port = 9008;
        }

        /// <summary>
        /// Gets or sets the path to adb.exe.
        /// </summary>
        public string AdbPath { get; set; }

        /// <summary>
        /// Gets or sets the serial of device under test.
        /// </summary>
        public string Serial { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should push and install
        /// all required dependencies.
        /// </summary>
        public DependencyHandling Dependencies { get; set; }

        /// <summary>
        /// Gets or sets the directory to all dependencies (uiautomator stub, helper apk, etc).
        /// </summary>
        public string DependenciesDirectory { get; set; }

        /// <summary>
        /// Gets or sets the local port.
        /// </summary>
        public int Port { get; set; }
    }
}
