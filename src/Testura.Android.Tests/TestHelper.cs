using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using Moq;
using Testura.Android.Device;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Tests
{
    internal class TestHelper
    {
        public TestHelper()
        {
            NodeFinderService = new Mock<INodeFinderService>();
            DeviceMock = new Mock<AndroidDevice>();
        }

        public Mock<INodeFinderService> NodeFinderService { get; }

        public Mock<AndroidDevice> DeviceMock { get; }

        public UiObject CreateUiObject(Where[] where, int delayInMiliSec, bool shouldThrowExeception = false)
        {
            if (shouldThrowExeception)
            {
                NodeFinderService.Setup(u => u.FindNodes(where, It.IsAny<TimeSpan>(), It.IsAny<string>()))
                    .Callback(() => Thread.Sleep(delayInMiliSec))
                    .Throws<UiNodeNotFoundException>();
            }
            else
            {
                NodeFinderService.Setup(u => u.FindNodes(where, It.IsAny<TimeSpan>(), It.IsAny<string>()))
                   .Callback(() => Thread.Sleep(delayInMiliSec))
                   .Returns(new List<Node> { new Node(new XElement("mm"), null) });
            }

            return new UiObject(null, NodeFinderService.Object, where, null);
        }
    }
}
