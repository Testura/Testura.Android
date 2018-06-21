using System;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Logging;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceLogger.AddListener(new ConsoleLogListener(DeviceLogger.LogLevels.Debug));

            var device = new AndroidDevice();
            device.CreateUiObject(With.Text("Chrome")).Tap();

            Console.ReadLine();
        }
    }
}
