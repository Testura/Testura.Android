using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Services;
using Testura.Android.UiAutomator;

namespace Testura.Android.Tests.UiAutomator
{
    [TestFixture]
    public class UiObjectTests
    {
        private Mock<IUiService> uiServiceMock; 
        private Mock<IDeviceServices> deviceServicesMock;

        [SetUp]
        public void SetUp()
        {
            uiServiceMock = new Mock<IUiService>();
            deviceServicesMock = new Mock<IDeviceServices>();
            deviceServicesMock.Setup(d => d.Ui).Returns(uiServiceMock.Object);
        }

        [Test]
        public void UiObject_WhenWaitingForNodeThatExist_ShouldGetNode()
        {
            var by = By.Text("Kalkylator");
            uiServiceMock.Setup(u => u.GetNodeBy(by, It.IsAny<int>()))
                .Returns(new Node(new XElement("node", new XAttribute("text", "Kalkylator"))));
            var uiObject = new UiObject(deviceServicesMock.Object, by);
            uiObject.WaitVisible();
        }
    }
}
