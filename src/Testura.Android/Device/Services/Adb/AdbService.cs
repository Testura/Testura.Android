using System;
using System.Collections.Generic;
using Testura.Android.Util;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Recording;

namespace Testura.Android.Device.Services.Adb
{
    /// <summary>
    /// Provides functionality to interact with adb on an android device.
    /// </summary>
    public class AdbService : IAdbShellService, IAdbInstallService
    {
        private readonly Terminal _terminal;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdbService"/> class.
        /// </summary>
        /// <param name="terminal">Object to interact with terminal</param>
        public AdbService(Terminal terminal)
        {
            _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));
        }

        /// <summary>
        /// Issue an adb command in the target emulator/device instance
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>The result from executing the command</returns>
        public string Execute(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentException("Argument is null or empty", nameof(command));
            }

            return ExecuteCommand(command);
        }

        /// <summary>
        /// Issues a shell command in the target emulator/device instance and then exits the remote shell
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>The result from executing the command</returns>
        public string Shell(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentException("Argument is null or empty", nameof(command));
            }

            return ExecuteCommand("shell", command);
        }

        /// <summary>
        /// Copies a specified file from your development computer to an emulator/device instance.
        /// </summary>
        /// <param name="localPath">Path to local file</param>
        /// <param name="remotePath">Path to destination on device/emulator</param>
        /// <exception cref="AdbException">Thrown when fail to push file</exception>
        public void Push(string localPath, string remotePath)
        {
            if (string.IsNullOrEmpty(localPath))
            {
                throw new ArgumentException("Argument is null or empty", nameof(localPath));
            }

            if (string.IsNullOrEmpty(remotePath))
            {
                throw new ArgumentException("Argument is null or empty", nameof(remotePath));
            }

            ExecuteCommand("push", localPath, remotePath);
        }

        /// <summary>
        /// Copies a specified file from an emulator/device instance to your development computer.
        /// </summary>
        /// <param name="remotePath">Path to file on device/emulator</param>
        /// <param name="localPath">Path to destination on local computer</param>
        /// <exception cref="AdbException">Thrown when fail to pull file</exception>
        public void Pull(string remotePath, string localPath)
        {
            if (string.IsNullOrEmpty(remotePath))
            {
                throw new ArgumentException("Argument is null or empty", nameof(remotePath));
            }

            if (string.IsNullOrEmpty(localPath))
            {
                throw new ArgumentException("Argument is null or empty", nameof(localPath));
            }

            ExecuteCommand("pull", remotePath, localPath);
        }

        /// <summary>
        /// Dump logcat to a local file
        /// </summary>
        /// <param name="localPath">Path to destination on local computer</param>
        public void DumpLogcat(string localPath)
        {
            if (string.IsNullOrEmpty(localPath))
            {
                throw new ArgumentException("Argument is null or empty", nameof(localPath));
            }

            ExecuteCommand("logcat", "-d ", ">", localPath);
        }

        /// <summary>
        /// Pushes an Android application (specified as a full path to an APK file) to an emulator/device.
        /// </summary>
        /// <param name="path">Full path to apk</param>
        /// <param name="shouldReinstall">True if we should use the reinstall flag </param>
        /// <param name="shouldUsePm">True if we should use the package manager flag</param>
        public void InstallApp(string path, bool shouldReinstall = true, bool shouldUsePm = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Argument is null or empty", nameof(path));
            }

            var arguments = new List<string>();

            if (shouldUsePm)
            {
                arguments.Add("shell");
                arguments.Add("pm");
            }

            arguments.Add("install");

            if (shouldReinstall)
            {
                arguments.Add("-r");
            }

            arguments.Add(path);

            ExecuteCommand(arguments.ToArray());
        }

        /// <summary>
        /// Take a screenshot of device display
        /// </summary>
        /// <param name="localPath">Save path for file</param>
        public void Screencap(string localPath)
        {
            if (string.IsNullOrEmpty(localPath))
            {
                throw new ArgumentException("Argument is null or empty", nameof(localPath));
            }

            var remotePath = "sdcard/temp_screencap_1234.png";
            ExecuteCommand("shell", "screencap", remotePath);
            Pull(remotePath, localPath);
            ExecuteCommand("shell", "rm", remotePath);
        }

        /// <summary>
        /// Recording the display of device with 3 min time limit.
        ///
        /// Will terminate any ongoing recordings.
        /// </summary>
        /// <param name="temporaryDeviceDirectory">Path to directory on device to temporary store the recording file before pulling.</param>
        /// <returns>The created screen recorder task (later used to stop the recording)</returns>
        public ScreenRecorderTask RecordScreen(string temporaryDeviceDirectory = "/sdcard/")
        {
            return RecordScreen(new ScreenRecordConfiguration(), temporaryDeviceDirectory);
        }

        /// <summary>
        /// Recording the display of device
        /// 
        /// Will terminate any ongoing recordings.
        /// </summary>
        /// <param name="configuration">Screen recording configuration (time limit, bit rate and size)</param>
        /// <param name="temporaryDeviceDirectory">Path to directory on device to temporary store the recording file before pulling.</param>
        /// <returns>The created screen recorder task (later used to stop the recording)</returns>
        public ScreenRecorderTask RecordScreen(ScreenRecordConfiguration configuration, string temporaryDeviceDirectory = "/sdcard/")
        {
            var screenRecorderTask = new ScreenRecorderTask(this, _terminal, temporaryDeviceDirectory);
            screenRecorderTask.StartRecording(configuration);
            return screenRecorderTask;
        }

        private string ExecuteCommand(params string[] arguments)
        {
            return _terminal.ExecuteAdbCommand(arguments);
        }
    }
}