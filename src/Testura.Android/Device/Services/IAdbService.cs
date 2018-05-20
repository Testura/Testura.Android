#pragma warning disable IDE0005 // Using directive is unnecessary.
using System;
using Testura.Android.Util;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Recording;

#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace Testura.Android.Device.Services
{
    /// <summary>
    /// Defines methods to interact with adb on an android device.
    /// </summary>
    public interface IAdbService
    {
        /// <summary>
        /// Issues a shell command in the target emulator/device instance and then exits the remote shell.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>The result from executing the command.</returns>
        string Shell(string command);

        /// <summary>
        /// Copies a specified file from your development computer to an emulator/device instance.
        /// </summary>
        /// <param name="localPath">Path to local file.</param>
        /// <param name="remotePath">Path to destination on device/emulator.</param>
        /// <exception cref="AdbException">Thrown when fail to push file.</exception>
        void Push(string localPath, string remotePath);

        /// <summary>
        /// Copies a specified file from an emulator/device instance to your development computer.
        /// </summary>
        /// <param name="remotePath">Path to file on device/emulator.</param>
        /// <param name="localPath">Path to destination on local computer.</param>
        /// <exception cref="AdbException">Thrown when fail to pull file.</exception>
        void Pull(string remotePath, string localPath);

        /// <summary>
        /// Dump logcat to a local file.
        /// </summary>
        /// <param name="localPath">Path to destination on local computer.</param>
        void DumpLogcat(string localPath);

        /// <summary>
        /// Pushes an Android application (specified as a full path to an APK file) to an emulator/device.
        /// </summary>
        /// <param name="path">Full path to apk.</param>
        /// <param name="shouldReinstall">True if we should use the reinstall flag.</param>
        /// <param name="shouldUsePm">True if we should use the package manager flag.</param>
        void InstallApp(string path, bool shouldReinstall = true, bool shouldUsePm = false);

        /// <summary>
        /// Take a screenshot of device display.
        /// </summary>
        /// <param name="localPath">Save path for file.</param>
        void Screencap(string localPath);


        /// <summary>
        /// Recording the display of device with 3 min time limit.
        ///
        /// Will terminate any ongoing recordings.
        /// </summary>
        /// <param name="temporaryDeviceDirectory">Path to directory on device to temporary store the recording file before pulling.</param>
        /// <returns>The created screen recorder task (later used to stop the recording)</returns>
        ScreenRecorderTask RecordScreen(string temporaryDeviceDirectory = "/sdcard/");

        /// <summary>
        /// Recording the display of device
        /// 
        /// Will terminate any ongoing recordings.
        /// </summary>
        /// <param name="configuration">Screen recording settings (time limit, bit rate and size)</param>
        /// <param name="temporaryDeviceDirectory">Path to directory on device to temporary store the recording file before pulling.</param>
        /// <returns>The created screen recorder task (later used to stop the recording)</returns>
        ScreenRecorderTask RecordScreen(ScreenRecordConfiguration configuration, string temporaryDeviceDirectory = "/sdcard/");
    }
}