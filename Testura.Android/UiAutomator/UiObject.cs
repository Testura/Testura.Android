using System;
using Testura.Android.Device;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.UiAutomator
{
    public class UiObject
    {
        private readonly IDeviceServices deviceServices;
        private readonly By by;

        public UiObject(IDeviceServices deviceServices, By by)
        {
            this.deviceServices = deviceServices;
            this.by = by;
        }

        public UiObject WaitVisible(int timeout = 20)
        {
            deviceServices.Ui.GetNodeBy(by, timeout);
            return this;
        }

        public void WaitNotVisible(int timeout = 20)
        {
            var startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    deviceServices.Ui.GetNodeBy(by, 0);
                    if ((DateTime.Now - startTime).Seconds > timeout)
                    {
                        throw new UiNodeNotFoundException(null);
                    }
                }
                catch (UiNodeNotFoundException)
                {
                    return;
                }
            }
        }

        public UiObject Click()
        {
            var node = deviceServices.Ui.GetNodeBy(by);
            var center = node.GetNodeCenter();
            deviceServices.Adb.Shell($"input tap {center.X} {center.Y}");
            return this;
        }
    }
}
