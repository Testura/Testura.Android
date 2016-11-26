using System.Diagnostics;

namespace Testura.Android.Util
{
    public interface ITerminal
    {
        string ExecuteCommand(string command);

        Process StartTerminalProcess(string command, bool useShell = true);
    }
}
