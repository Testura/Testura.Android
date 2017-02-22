using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using NUnit.Framework;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Tests.Device.UiAutomator.Ui.Objects
{
    [TestFixture]
    public class UiObjectTests
    {
        private TestHelper _testHelper;

        [SetUp]
        public void SetUp()
        {
            _testHelper = new TestHelper();
        }

        [Test]
        public void WaitForValue_WhenWaitingForValueAndValuesMatch_ShouldReturnTrue()
        {
            var uiObject = _testHelper.CreateUiObject(With.Class("testClass"), 0);

            _testHelper.UiServiceMock.Setup(u => u.FindNode(It.IsAny<int>(), It.IsAny<With[]>()))
                .Returns(new Node(new XElement("node", new XAttribute("clickable", "true")), null));

            Assert.IsTrue(uiObject.WaitForValue(n => n.Clickable));
        }

        [Test]
        public void WaitForValue_WhenWaitingForValueAndValuesDontMatch_ShouldReturnFalse()
        {
            var uiObject = _testHelper.CreateUiObject(With.Class("testClass"), 0);

            _testHelper.UiServiceMock.Setup(u => u.FindNode(It.IsAny<int>(), It.IsAny<With[]>()))
                .Returns(new Node(new XElement("node", new XAttribute("clickable", "false")), null));

            Assert.IsFalse(uiObject.WaitForValue(n => n.Clickable, 1));
        }

        [Test]
        public void WaitForValue_WhenWaitingForValueAndValueChangeAfter2Second_ShouldReturnTrue()
        {
            var uiObject = _testHelper.CreateUiObject(With.Class("testClass"), 0);

            _testHelper.UiServiceMock.Setup(u => u.FindNode(It.IsAny<int>(), It.IsAny<With[]>()))
                .Returns(new Node(new XElement("node", new XAttribute("clickable", "false")), null));

            Task.Run(() =>
            {
                Thread.Sleep(2000);
                _testHelper.UiServiceMock.Setup(u => u.FindNode(It.IsAny<int>(), It.IsAny<With[]>()))
                    .Returns(new Node(new XElement("node", new XAttribute("clickable", "true")), null));
            });

            Assert.IsTrue(uiObject.WaitForValue(n => n.Clickable));
        }

    }
}