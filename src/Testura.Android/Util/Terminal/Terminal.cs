using System;
using System.Collections.Generic;
using System.ComponentModel;
using Medallion.Shell;
using Testura.Android.Device.Configurations;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util.Terminal
{
    public class Terminal : ITerminal
    {
        private const string AdbNotFoundError = "Could not find adb.exe. Make sure that Android SDK are installed and that you have adb in your windows environment variables or specificed the path to adb.exe inside your device configuration.";
        private readonly DeviceConfiguration _deviceConfiguration;

        public Terminal(DeviceConfiguration deviceConfiguration)
        {
            _deviceConfiguration = deviceConfiguration;
        }

        /// <summary>
        /// Execute a new adb command
        /// </summary>
        /// <param name="arguments">Arguments to send to the adb</param>
        /// <returns>Output from adb</returns>
        public string ExecuteAdbCommand(params string[] arguments)
        {
            var allArguments = new List<string>();
            if (!string.IsNullOrEmpty(_deviceConfiguration.Serial))
            {
                allArguments.Add("-s");
                allArguments.Add(_deviceConfiguration.Serial);
            }

            allArguments.AddRange(arguments);

            DeviceLogger.Log($"Sending adb command: {string.Join(" ", allArguments)}");

            try
            {
                using (var command = Command.Run(
                    "adb.exe",
                    allArguments,
                    options: o => o.Timeout(TimeSpan.FromMinutes(1))))
                {
                    var output = command.StandardOutput.ReadToEnd();
                    var error = command.StandardError.ReadToEnd();

                    if (!command.Result.Success)
                    {
                        var message = $"Output: {output}, Error: {error}";
                        DeviceLogger.Log(message);
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
        /// Start the adb process and return the command
        /// </summary>
        /// <param name="arguments">Arguments that should be provided to adb</param>
        /// <returns>The command that contains the started process</returns>
        public Command StartAdbProcess(params string[] arguments)
        {
            var allArguments = new List<string> { "/c", GetAdbExe() };

            if (!string.IsNullOrEmpty(_deviceConfiguration.Serial))
            {
                allArguments.Add("-s");
                allArguments.Add(_deviceConfiguration.Serial);
            }

            allArguments.AddRange(arguments);

            DeviceLogger.Log($"Starting adb process with shell: {string.Join(" ", allArguments)}");

            try
            {
                var command = Command.Run(
                    "cmd.exe",
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
        /// Start the adb process without shell and no window and return the command
        /// </summary>
        /// <param name="arguments">Arguments that should be provided to adb</param>
        /// <returns>The command that contains the started process</returns>
        public Command StartAdbProcessWithoutShell(params string[] arguments)
        {
            var allArguments = new List<string> { "/c", GetAdbExe() };

            if (!string.IsNullOrEmpty(_deviceConfiguration.Serial))
            {
                allArguments.Add("-s");
                allArguments.Add(_deviceConfiguration.Serial);
            }

            allArguments.AddRange(arguments);

            DeviceLogger.Log($"Starting adb process without shell: {string.Join(" ", allArguments)}");

            try
            {
                var command = Command.Run(
                    "cmd.exe",
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
            if (string.IsNullOrEmpty(_deviceConfiguration.AdbPath))
            {
                return "adb.exe";
            }

            return _deviceConfiguration.AdbPath;
        }
    }
}
