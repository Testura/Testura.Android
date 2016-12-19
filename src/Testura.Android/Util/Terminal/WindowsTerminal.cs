using System.Diagnostics;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util.Terminal
{
    public class WindowsTerminal : ITerminal
    {
        /// <summary>
        /// Execute a terminal command and wait for it to finish.
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>Result back from terminal</returns>
        /// <exception cref="TerminalException">Thrown if terminal output contains error</exception>
        public string ExecuteCommand(string command)
        {
            DeviceLogger.Log($"Executing new terminal command: {command}");
            var cmd = StartTerminalProcess(command, false);
            cmd.WaitForExit();
            var output = cmd.StandardOutput.ReadToEnd();
            DeviceLogger.Log($"Terminal output: {output}");
            var errorOutput = cmd.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(errorOutput))
            {
                DeviceLogger.Log($"Errors found in terminal output! - {errorOutput}");
                throw new TerminalException(errorOutput);
            }

            return output;
        }

        /// <summary>
        /// Execute a terminal command and return the process back
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <param name="useShell">Set if we should use the operation system shell to start the process</param>
        /// <returns>The started terminal process</returns>
        public Process StartTerminalProcess(string command, bool useShell = true)
        {
            var cmd = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = !useShell,
                    RedirectStandardOutput = !useShell,
                    RedirectStandardError = !useShell,
                    CreateNoWindow = true,
                    UseShellExecute = useShell,
                    Arguments = $"/C {command}"
                }
            };
            cmd.Start();
            return cmd;
        }
    }
}
