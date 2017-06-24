using Medallion.Shell;
#pragma warning disable IDE0005 // Using directive is unnecessary.

#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace Testura.Android.Util.Terminal
{
    /// <summary>
    /// Define methods to interact with the terminal.
    /// </summary>
    public interface ITerminal
    {
        /// <summary>
        /// Execute a new adb command
        /// </summary>
        /// <param name="arguments">Arguments to send to the adb</param>
        /// <returns>Output from adb</returns>
        string ExecuteAdbCommand(params string[] arguments);

        /// <summary>
        /// Start the adb process and return the command
        /// </summary>
        /// <param name="arguments">Arguments that should be provided to adb</param>
        /// <returns>The command that contains the started process</returns>
        Command StartAdbProcess(params string[] arguments);

        /// <summary>
        /// Start the adb process without shell and return the command
        /// </summary>
        /// <param name="arguments">Arguments that should be provided to adb</param>
        /// <returns>The command that contains the started process</returns>
        Command StartAdbProcessWithoutShell(params string[] arguments);
    }
}
