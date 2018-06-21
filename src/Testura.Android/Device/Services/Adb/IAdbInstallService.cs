namespace Testura.Android.Device.Services.Adb
{
    /// <summary>
    /// Defines method(s) to handle installation of apps
    /// </summary>
    public interface IAdbInstallService
    {
        /// <summary>
        /// Pushes an Android application (specified as a full path to an APK file) to an emulator/device.
        /// </summary>
        /// <param name="path">Full path to apk</param>
        /// <param name="shouldReinstall">True if we should use the reinstall flag </param>
        /// <param name="shouldUsePm">True if we should use the package manager flag</param>
        void InstallApp(string path, bool shouldReinstall = true, bool shouldUsePm = false);
    }
}
