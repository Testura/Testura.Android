using System;
using Testura.Android.Device;
using Testura.Android.UiAutomator;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Services
{
    public class UiService : Service, IUiService
    {
        private readonly INodeChecker nodeChecker;

        public UiService(IDeviceServices deviceServices, INodeChecker nodeChecker)
            : base(deviceServices)
        {
            this.nodeChecker = nodeChecker;
        }

        public UiService(INodeChecker nodeChecker)
        {
            this.nodeChecker = nodeChecker;
        }

        public Node GetNodeBy(By by, int timeout = 2)
        {
            var startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    var node = nodeChecker.GetNodeBy(by);
                    return new Node(node);
                }
                catch (UiNodeNotFoundException)
                {
                    if ((DateTime.Now - startTime).Seconds < timeout)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
