using System;
using System.CodeDom;
using System.Diagnostics;

namespace Testura.Android.Util
{
    public class WindowsTerminal : ITerminal
    {
        public string ExecuteCommand(string command)
        {
            var cmd = StartTerminalProcess(command, false);
            cmd.WaitForExit();
            return cmd.StandardOutput.ReadToEnd();
        }

        public Process StartTerminalProcess(string command, bool useShell = true)
        {
            var cmd = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = !useShell,
                    RedirectStandardOutput = !useShell,
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
