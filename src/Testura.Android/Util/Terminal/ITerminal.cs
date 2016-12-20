using System.Diagnostics;
using Medallion.Shell;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using Testura.Android.Util.Exceptions;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace Testura.Android.Util.Terminal
{
    public interface ITerminal
    {
        /// <summary>
        /// Execute a terminal command and wait for it to finish.
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>Result back from terminal</returns>
        /// <exception cref="TerminalException">Thrown if terminal output contains error</exception>
        string ExecuteAdbCommand(params string[] arguments);

        /// <summary>
        /// Execute a terminal command and return the process back
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <param name="useShell">Set if we should use the operation system shell to start the process</param>
        /// <returns>The started terminal process</returns>
        Command StartAdbProcess(params string[] arguments);
    }
}
