using System.Threading;
using System.Xml.Linq;
using Moq;
using Testura.Android.Device;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Tests
{
    internal class TestHelper
    {
        private readonly UiService _uiService;

        public TestHelper()
        {
            DeviceMock = new Mock<AndroidDevice>();
            UiServiceMock = new Mock<INodeFinderService>();
            DeviceMock.SetupGet(d => d.Ui).Returns(UiServiceMock.Object);

            _uiService = new UiService(
                new Mock<IScreenDumper>().Object,
                new Mock<INodeParser>().Object,
                new Mock<INodeFinder>().Object);
        }

        public Mock<INodeFinderService> UiServiceMock { get; }

        public Mock<AndroidDevice> DeviceMock { get; }

        public UiObject CreateUiObject(With with, int delayInMiliSec, bool shouldThrowExeception = false)
        {
            if (shouldThrowExeception)
            {
                UiServiceMock.Setup(u => u.FindNode(It.IsAny<int>(), with))
                    .Callback(() => Thread.Sleep(delayInMiliSec))
                    .Throws<UiNodeNotFoundException>();
            }
            else
            {
                UiServiceMock.Setup(u => u.FindNode(It.IsAny<int>(), with))
                   .Callback(() => Thread.Sleep(delayInMiliSec))
                   .Returns(new Node(new XElement("mm"), null));
            }
            return DeviceMock.Object.CreateUiObject(with);
        }

        public UiObjects CreateUiObjects(With with, int delayInMiliSec, bool shouldThrowExeception = false)
        {
            if (shouldThrowExeception)
            {
                UiServiceMock.Setup(u => u.FindNode(It.IsAny<int>(), with))
                    .Callback(() => Thread.Sleep(delayInMiliSec))
                    .Throws<UiNodeNotFoundException>();
            }
            else
            {
                UiServiceMock.Setup(u => u.FindNode(It.IsAny<int>(), with))
                   .Callback(() => Thread.Sleep(delayInMiliSec))
                   .Returns(new Node(new XElement("mm"), null));
            }
            return DeviceMock.Object.CreateUiObjects(with);
        }
    }
}
