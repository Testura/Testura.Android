using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Testura.Android.Device.UiAutomator.Ui.Util;

namespace Testura.Android.Tests.Device.UiAutomator.Ui.Util
{
    [TestFixture]
    public class NodeParserTests
    {
        private NodeParser _nodeParser;

        [SetUp]
        public void SetUp()
        {
            _nodeParser = new NodeParser();
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetNodes()
        {
            var xDocument = new XDocument(new XElement("node"));
            var nodes =_nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(1, nodes.Count);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetTextAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("text", "test")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual("test", nodes.First().Text);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetIndexAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("index", 1)));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual("1", nodes.First().Index);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetContetDescAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("content-desc", "test")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual("test", nodes.First().ContentDesc);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetClassAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("class", "test")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual("test", nodes.First().Class);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetResourceIdAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("resource-id", "test")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual("test", nodes.First().ResourceId);
        }
    }
}
