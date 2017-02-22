using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using Testura.Android.Device;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Default;
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
        private Mock<IAndroidDevice> _deviceMock;

        public TestHelper()
        {
            _deviceMock = new Mock<IAndroidDevice>();
            UiServiceMock = new Mock<IUiService>();
            _deviceMock.SetupGet(d => d.Ui).Returns(UiServiceMock.Object);

            _uiService = new UiService(
                new Mock<IScreenDumper>().Object,
                new Mock<INodeParser>().Object,
                new Mock<INodeFinder>().Object);
            _uiService.InitializeServiceOwner(_deviceMock.Object);
        }

        public Mock<IUiService> UiServiceMock { get; private set; }

        public UiObject CreateUiObject(With with, int delayInMiliSec, bool shouldThrowExeception = false)
        {
            if (shouldThrowExeception)
            {
                UiServiceMock.Setup(u => u.FindNode(10, with))
                    .Callback(() => Thread.Sleep(delayInMiliSec))
                    .Throws<UiNodeNotFoundException>();
            }
            else
            {
                UiServiceMock.Setup(u => u.FindNode(10, with))
                   .Callback(() => Thread.Sleep(delayInMiliSec))
                   .Returns(new Node(new XElement("mm"), null));
            }
            return _uiService.CreateUiObject(with);
        }
    }
}
