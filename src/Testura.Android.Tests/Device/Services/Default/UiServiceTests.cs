using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Moq;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Services.Default;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Tests.Device.Services.Default
{
    [TestFixture]
    public class UiServiceTests
    {
        private Mock<IAndroidDevice> _deviceComponentOwner;
        private Mock<IScreenDumper> _screenDumper;
        private Mock<INodeFinder> _nodeFinder;
        private Mock<INodeParser> _nodeParser;
        private UiService _uiComponent;

        [SetUp]
        public void SetUp()
        {
            _deviceComponentOwner = new Mock<IAndroidDevice>();
            _screenDumper = new Mock<IScreenDumper>();
            _nodeParser = new Mock<INodeParser>();
            _nodeFinder = new Mock<INodeFinder>();
            _uiComponent = new UiService(_screenDumper.Object, _nodeParser.Object, _nodeFinder.Object);
            _uiComponent.InitializeServiceOwner(_deviceComponentOwner.Object);
        }

        [Test]
        public void FindNodes_WhenLookingForNode_ShouldCallOnCorrectServices()
        {
            With.Lambda(n => n.Text == "Some text" && n.Parent != null && n.Children.First().ContentDesc == "My child");

            var document = new XDocument();
            var nodes = new List<Node>();
            var with = new[] { With.Text("mm") };
            _screenDumper.Setup(s => s.DumpUi()).Returns(document);
            _nodeParser.Setup(n => n.ParseNodes(document)).Returns(nodes);
            _nodeFinder.Setup(n => n.FindNodes(nodes, with)).Returns(new List<Node> { new Node(new XElement("node"), null)});
            _uiComponent.FindNode(0, with);
            _screenDumper.Verify(s => s.DumpUi(), Times.Once);
            _nodeParser.Verify(n => n.ParseNodes(document), Times.Once);
            _nodeFinder.Verify(n => n.FindNodes(nodes, with), Times.Once);
        }
    }
}
