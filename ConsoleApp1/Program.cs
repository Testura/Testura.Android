using System;
using System.Threading;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util;
using Testura.Android.Util.Logging;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            DeviceLogger.AddListener(new ConsoleLogListener(DeviceLogger.LogLevels.Debug));
            var device = new AndroidDevice();

            while (true)
            {
                var recording = device.Adb.RecordScreen();
                for (int n = 0; n < 10; n++)
                {
                    device.Interaction.InputKeyEvent(KeyEvents.WakeUp);
                    Thread.Sleep(1000);
                }
                
                recording.StopRecording($@"C:\Users\Mille\OneDrive\Dokument\mm\{random.Next(0, 20000)}.mp4");
            }
        }
    }
}
