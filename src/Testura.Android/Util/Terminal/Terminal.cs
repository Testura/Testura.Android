using System;
using System.Collections.Generic;
using Medallion.Shell;
using Testura.Android.Device.Configurations;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util.Terminal
{
    public class Terminal : ITerminal
    {
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
        public string ExecuteAdbCommand(string[] arguments)
        {
            var allArguments = new List<string>();
            if (!string.IsNullOrEmpty(_deviceConfiguration.Serial))
            {
                allArguments.Add("-s");
                allArguments.Add(_deviceConfiguration.Serial);
            }

            allArguments.AddRange(arguments);

            DeviceLogger.Log($"Sending adb command with: {string.Join(" ", allArguments)}");

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

        /// <summary>
        /// Start the adb process and return the command
        /// </summary>
        /// <param name="arguments">Arguments that should be provided to adb</param>
        /// <returns>The command that contains the started process</returns>
        public Command StartAdbProcess(string[] arguments)
        {
            var allArguments = new List<string> { "/c", GetAdbExe() };

            if (!string.IsNullOrEmpty(_deviceConfiguration.Serial))
            {
                allArguments.Add("-s");
                allArguments.Add(_deviceConfiguration.Serial);
            }

            allArguments.AddRange(arguments);

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
