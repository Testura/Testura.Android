using System;
using System.Collections.Generic;
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
            var uiObject = _testHelper.CreateUiObject(new[] { Where.Class("testClass") }, 0);

            _testHelper.NodeFinderService.Setup(u => u.FindNodes(It.IsAny<Where[]>(), It.IsAny<TimeSpan>(), It.IsAny<string>()))
                .Returns(new List<Node>() { new Node(new XElement("node", new XAttribute("clickable", "true")), null) });

            Assert.IsTrue(uiObject.WaitUntil(n => n.Clickable));
        }

        [Test]
        public void WaitForValue_WhenWaitingForValueAndValuesDontMatch_ShouldReturnFalse()
        {
            var uiObject = _testHelper.CreateUiObject(new[] { Where.Class("testClass") }, 0);

            _testHelper.NodeFinderService.Setup(u => u.FindNodes(It.IsAny<Where[]>(), It.IsAny<TimeSpan>(), It.IsAny<string>()))
                .Returns(new List<Node>() { new Node(new XElement("node", new XAttribute("clickable", "false")), null) });

            Assert.IsFalse(uiObject.WaitUntil(n => n.Clickable, TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void WaitForValue_WhenWaitingForValueAndValueChangeAfter2Second_ShouldReturnTrue()
        {
            var uiObject = _testHelper.CreateUiObject(new[] { Where.Class("testClass") }, 0);

            _testHelper.NodeFinderService.Setup(u => u.FindNodes(It.IsAny<Where[]>(), It.IsAny<TimeSpan>(), It.IsAny<string>()))
                .Returns(new List<Node>() { new Node(new XElement("node", new XAttribute("clickable", "false")), null) });

            Task.Run(() =>
            {
                Thread.Sleep(2000);
                _testHelper.NodeFinderService.Setup(u => u.FindNodes(It.IsAny<Where[]>(), It.IsAny<TimeSpan>(), It.IsAny<string>()))
                    .Returns(new List<Node>() { new Node(new XElement("node", new XAttribute("clickable", "true")), null) });
            });

            Assert.IsTrue(uiObject.WaitUntil(n => n.Clickable));
        }
    }
}
