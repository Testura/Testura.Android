namespace Testura.Android.Device.Services.Adb
{
    public interface IAdbShellService
    {
        /// <summary>
        /// Issues a shell command in the target emulator/device instance and then exits the remote shell
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>The result from executing the command</returns>
        string Shell(string command);
    }
}
