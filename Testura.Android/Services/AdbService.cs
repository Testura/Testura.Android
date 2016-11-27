using Testura.Android.Device;
using Testura.Android.Util;

namespace Testura.Android.Services
{
    public class AdbService : Service, IAdbService
    {
        private readonly ITerminal terminal;

        public AdbService(IDeviceServices deviceServices, ITerminal terminal)
            : base(deviceServices)
        {
            this.terminal = terminal;
        }

        public AdbService(ITerminal terminal)
        {
            this.terminal = terminal;
        }

        public string Shell(string command)
        {
            return terminal.ExecuteCommand($"adb shell {command}");
        }
    }
}
