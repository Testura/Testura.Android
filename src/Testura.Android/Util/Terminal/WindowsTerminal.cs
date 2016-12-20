using System;
using System.Collections.Generic;
using Medallion.Shell;
using Testura.Android.Device.Configurations;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util.Terminal
{
    public class WindowsTerminal : ITerminal
    {
        private readonly DeviceConfiguration _deviceConfiguration;

        public WindowsTerminal(DeviceConfiguration deviceConfiguration)
        {
            _deviceConfiguration = deviceConfiguration;
        }

        public string ExecuteAdbCommand(string[] arguments)
        {
            var allArguments = new List<string>();
            if (!string.IsNullOrEmpty(_deviceConfiguration.Serial))
            {
                allArguments.Add("-s");
                allArguments.Add(_deviceConfiguration.Serial);
            }

            allArguments.AddRange(arguments);

            using (var command = Command.Run(
                "adb.exe",
                arguments,
                options: o => o.Timeout(TimeSpan.FromMinutes(1))))
            {

                var output = command.StandardOutput.ReadToEnd();
                var error = command.StandardOutput.ReadToEnd();

                if (!command.Result.Success)
                {
                    DeviceLogger.Log($"Error: {error}");
                    throw new AdbException(error);
                }

                return output;
            }
        }

        public Command StartAdbProcess(string[] arguments)
        {
            var command = Command.Run(
                GetAdbExe(),
                arguments,
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
