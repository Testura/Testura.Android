using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Device.Services.Default
{
    public class AdbService : Service, IAdbService
    {
        private readonly ITerminal _terminal;

        public AdbService(ITerminal terminal)
        {
            _terminal = terminal;
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

            return ExecuteCommand("shell", $"\"{command}\"");
        }

        /// <summary>
        /// Copies a specified file from your development computer to an emulator/device instance.
        /// </summary>
        /// <param name="localPath">Path to local file</param>
        /// <param name="remotePath">Path to destionation on device/emulator</param>
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

            ExecuteCommand("push", $"\"{localPath}\"", $"\"{remotePath}\"");
        }

        /// <summary>
        /// Copies a specified file from an emulator/device instance to your development computer.
        /// </summary>
        /// <param name="remotePath">Path to file on device/emulato</param>
        /// <param name="localPath">Path to destionation on local computer</param>
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

            ExecuteCommand("pull", $"\"{localPath}\"", $"\"{remotePath}\"");
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

            ExecuteCommand("logcat", "-d ", ">",  $"\"{localPath}\"");
        }

        /// <summary>
        /// Pushes an Android application (specified as a full path to an APK file) to an emulator/device.
        /// </summary>
        /// <param name="path">Full path to apk</param>
        /// <param name="shouldReinstall">True if we should use the reinstall flag </param>
        public void InstallApp(string path, bool shouldReinstall = true)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Argument is null or empty", nameof(path));
            }

            ExecuteCommand("install", shouldReinstall ? "-r" : string.Empty, $"\"{path}\"");
        }

        private string ExecuteCommand(params string[] arguments)
        {
            var command = new List<string> { GetAdb(), GetSerial() };
            command.AddRange(arguments);
            var finalCommand = string.Join(" ", command.Where(a => !string.IsNullOrEmpty(a)));
            DeviceLogger.Log($"Sending adb command: {finalCommand}");
            var result = _terminal.ExecuteCommand(finalCommand);
            if (result.Contains("error"))
            {
                DeviceLogger.Log($"Result from command contains error: {result}");
                throw new AdbException(result);
            }

            return result;
        }

        private string GetAdb()
        {
            if (string.IsNullOrEmpty(Device.Configuration.AdbPath))
            {
                return "adb";
            }

            return Device.Configuration.AdbPath;
        }

        private string GetSerial()
        {
            if (!string.IsNullOrEmpty(Device.Configuration.Serial))
            {
                return $"-s {Device.Configuration.Serial}";
            }

            return string.Empty;
        }
    }
}
