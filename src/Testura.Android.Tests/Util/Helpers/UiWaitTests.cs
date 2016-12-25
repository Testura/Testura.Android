using System.Threading;
using System.Xml.Linq;
using Moq;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Default;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Helpers;

namespace Testura.Android.Tests.Util.Helpers
{
    [TestFixture]
    public class UiWaitTests
    {
        private Mock<IAndroidDevice> _deviceMock;
        private Mock<IUiService> _uiServiceMock;

        private UiService _uiService;

        [SetUp]
        public void SetUp()
        {
            _deviceMock = new Mock<IAndroidDevice>();
            _uiServiceMock = new Mock<IUiService>();
            _deviceMock.SetupGet(d => d.Ui).Returns(_uiServiceMock.Object);
            
            _uiService = new UiService(
                new Mock<IScreenDumper>().Object, 
                new Mock<INodeParser>().Object, 
                new Mock<INodeFinder>().Object);
            _uiService.InitializeServiceOwner(_deviceMock.Object);
        }

        [Test]
        public void ForAny_WhenFindNodeOnFirst_ShouldReturnFirst()
        {
            var uiObjectOne = CreateUiObject(With.Class("test"), 0);
            var uiObjectTwo = CreateUiObject(With.ResourceId("test"), 2000);
            var foundObject = UiWait.ForAny(10, uiObjectOne.IsVisible, uiObjectTwo.IsHidden);
            Assert.AreEqual(uiObjectOne, foundObject);
        }

        [Test]
        public void ForAny_IfNoObjectIsFound_ShouldThrowException()
        {
            var uiObject = CreateUiObject(With.Class("hej"), 0, true);
            Assert.Throws<UiNodeNotFoundException>(() => UiWait.ForAny(10, uiObject.IsVisible));
        }

        [Test]
        public void ForAny_IfObjectLaterThrowsException_ShouldNotCrash()
        {
            var uiObjectOne = CreateUiObject(With.Class("test"), 0);
            var uiObjectTwo = CreateUiObject(With.ResourceId("test"), 500, true);
            var foundObject = UiWait.ForAny(10, uiObjectOne.IsVisible, uiObjectTwo.IsHidden);
            Assert.AreEqual(uiObjectOne, foundObject);
            Thread.Sleep(1000);
        }

        private UiObject CreateUiObject(With with, int delayInMiliSec, bool shouldThrowExeception = false)
        {
            if (shouldThrowExeception)
            {
                _uiServiceMock.Setup(u => u.FindNode(10, with))
                    .Callback(() => Thread.Sleep(delayInMiliSec))
                    .Throws<UiNodeNotFoundException>();
            }
            else
            {
                _uiServiceMock.Setup(u => u.FindNode(10, with))
                   .Callback(() => Thread.Sleep(delayInMiliSec))
                   .Returns(new Node(new XElement("mm"), null));
            }
            return _uiService.CreateUiObject(with);
        }
    }
}
