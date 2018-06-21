using System;
using System.Collections.Generic;
using System.ComponentModel;
using Medallion.Shell;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util
{
    /// <summary>
    /// Provides functionality to interact with the terminal.
    /// </summary>
    public class Terminal
    {
        private const string AdbNotFoundError = "Could not find adb.exe. Make sure that Android SDK are installed and that you have adb in your windows environment variables or specified the path to adb.exe inside your device configuration.";
        private readonly string _serial;
        private readonly string _adbPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="Terminal"/> class.
        /// </summary>
        /// <param name="serial">Serial number of device</param>
        /// <param name="adbPath">Path to adb.exe</param>
        public Terminal(string serial = null, string adbPath = null )
        {
            _serial = serial;
            _adbPath = adbPath;
        }

        /// <summary>
        /// Execute a new adb command.
        /// </summary>
        /// <param name="arguments">Arguments to send to the adb.</param>
        /// <returns>Output from adb.</returns>
        public string ExecuteAdbCommand(params string[] arguments)
        {
            var allArguments = new List<string>();

            if (!string.IsNullOrEmpty(_serial))
            {
                allArguments.Add("-s");
                allArguments.Add(_serial);
            }

            allArguments.AddRange(arguments);

            DeviceLogger.Log($"Sending adb command: {string.Join(" ", allArguments)}", DeviceLogger.LogLevels.Info);

            try
            {
                using (var command = Command.Run(
                    GetAdbExe(),
                    allArguments,
                    options: o => o.Timeout(TimeSpan.FromMinutes(1))))
                {
                    var output = command.StandardOutput.ReadToEnd();
                    var error = command.StandardError.ReadToEnd();

                    if (!command.Result.Success)
                    {
                        var message = $"Output: {output}, Error: {error}";
                        DeviceLogger.Log(message, DeviceLogger.LogLevels.Error);
                        throw new AdbException(message);
                    }

                    return output;
                }
            }
            catch (Win32Exception)
            {
                throw new AdbException(AdbNotFoundError);
            }
        }

        /// <summary>
        /// Start the adb process and return the command.
        /// </summary>
        /// <param name="arguments">Arguments that should be provided to adb.</param>
        /// <returns>The command that contains the started process.</returns>
        public Command StartAdbProcess(params string[] arguments)
        {
            var allArguments = new List<string>();

            if (!string.IsNullOrEmpty(_serial))
            {
                allArguments.Add("-s");
                allArguments.Add(_serial);
            }

            allArguments.AddRange(arguments);

            DeviceLogger.Log($"Starting adb process with shell: {string.Join(" ", allArguments)}", DeviceLogger.LogLevels.Info);

            try
            {
                var command = Command.Run(
                    GetAdbExe(),
                    allArguments.ToArray(),
                    o =>
                    {
                        o.StartInfo(si =>
                        {
                            si.CreateNoWindow = false;
                            si.UseShellExecute = true;
                            si.RedirectStandardError = false;
                            si.RedirectStandardInput = false;
                            si.RedirectStandardOutput = false;
                        });
                        o.DisposeOnExit(false);
                    });
                return command;
            }
            catch (Win32Exception)
            {
                throw new AdbException(AdbNotFoundError);
            }
        }

        /// <summary>
        /// Start the adb process without shell and no window and return the command.
        /// </summary>
        /// <param name="arguments">Arguments that should be provided to adb.</param>
        /// <returns>The command that contains the started process.</returns>
        public Command StartAdbProcessWithoutShell(params string[] arguments)
        {
            var allArguments = new List<string>();

            if (!string.IsNullOrEmpty(_serial))
            {
                allArguments.Add("-s");
                allArguments.Add(_serial);
            }

            allArguments.AddRange(arguments);

            DeviceLogger.Log($"Starting adb process without shell: {string.Join(" ", allArguments)}", DeviceLogger.LogLevels.Info);

            try
            {
                var command = Command.Run(
                    GetAdbExe(),
                    allArguments.ToArray(),
                    o =>
                    {
                        o.StartInfo(si =>
                        {
                            si.CreateNoWindow = true;
                            si.UseShellExecute = false;
                            si.RedirectStandardError = true;
                            si.RedirectStandardInput = true;
                            si.RedirectStandardOutput = true;
                        });
                        o.DisposeOnExit();
                    });
                return command;
            }
            catch (Win32Exception)
            {
                throw new AdbException(AdbNotFoundError);
            }
        }

        private string GetAdbExe()
        {
            if (string.IsNullOrEmpty(_adbPath))
            {
                return "adb.exe";
            }

            return _adbPath;
        }
    }
}
