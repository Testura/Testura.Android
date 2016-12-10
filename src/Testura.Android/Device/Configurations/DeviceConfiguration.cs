using System.IO;
using System.Reflection;
using Testura.Android.Util.Extensions;

namespace Testura.Android.Device.Configurations
{
    public class DeviceConfiguration
    {
        private const string ServerAApkName = "testura.android.server.a.apk";
        private const string ServerBApkName = "testura.android.server.b.apk";

        public DeviceConfiguration()
        {
            AdbPath = string.Empty;
            Serial = string.Empty;
            ShouldInstallApk = true;
            ServerApkPath = Path.Combine(Assembly.GetExecutingAssembly().GetDirectoryPath(), "Apk", ServerAApkName);
            HelperApkPath = Path.Combine(Assembly.GetExecutingAssembly().GetDirectoryPath(), "Apk", ServerBApkName);
            Port = 9008;
        }

        /// <summary>
        /// Gets or sets the path to adb.exe
        /// </summary>
        public string AdbPath { get; set; }

        /// <summary>
        /// Gets or sets the serial of device under test.
        /// </summary>
        public string Serial { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should install
        /// server and help apks at startup.
        /// at startup.
        /// </summary>
        public bool ShouldInstallApk { get; set; }

        /// <summary>
        /// Gets or sets the path to the server apk
        /// </summary>
        public string ServerApkPath { get; set; }

        /// <summary>
        /// Gets or sets the path to the helper apk
        /// </summary>
        public string HelperApkPath { get; set; }

        /// <summary>
        /// Gets or sets the local port to the device
        /// </summary>
        public int Port { get; set; }
    }
}
