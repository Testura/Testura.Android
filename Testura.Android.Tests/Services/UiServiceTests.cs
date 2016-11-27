using System.Xml.Linq;
using Moq;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Services;
using Testura.Android.UiAutomator;

namespace Testura.Android.Tests.Services
{
    [TestFixture]
    public class UiServiceTests
    {
        private Mock<IDeviceServices> deviceServicesMock;
        private Mock<INodeChecker> nodeCheckerMock;
        private UiService uiService;

        [SetUp]
        public void SetUp()
        {
            deviceServicesMock = new Mock<IDeviceServices>();
            nodeCheckerMock = new Mock<INodeChecker>();
            uiService = new UiService(deviceServicesMock.Object, nodeCheckerMock.Object);
        }

        [Test]
        public void UiService_WhenGettingNodeByText_ShouldGetNodeAndTextAttriuteShouldBeCorrect()
        {
            var by = By.Text("Kalkylator");
            nodeCheckerMock.Setup(n => n.GetNodeBy(by)).Returns(new XElement("node", new XAttribute("text", "Kalkylator")));
            var node = uiService.GetNodeBy(by);
            Assert.IsNotNull(node);
            Assert.AreEqual("Kalkylator", node.Text);
        }
    }
}
