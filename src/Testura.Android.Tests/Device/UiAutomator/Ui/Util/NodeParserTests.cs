using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Testura.Android.Device.Ui.Nodes;

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
            var nodes = _nodeParser.ParseNodes(xDocument);
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

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetCheckableAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("checkable", "true")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(true, nodes.First().Checkable);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetCheckedAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("checked", "true")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(true, nodes.First().Checked);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetClickabledAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("clickable", "true")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(true, nodes.First().Clickable);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetEnabledAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("enabled", "true")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(true, nodes.First().Enabled);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetFocusableAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("focusable", "true")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(true, nodes.First().Focusable);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetFocusedAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("focused", "true")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(true, nodes.First().Focused);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldGetScrollableAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("scrollable", "true")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(true, nodes.First().Scrollable);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldLongClickableGetAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("long-clickable", "true")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(true, nodes.First().LongClickable);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldPasswordGetAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("password", "true")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(true, nodes.First().Password);
        }

        [Test]
        public void ParseNodes_WhenParsingXDocument_ShouldSelectedGetAttribute()
        {
            var xDocument = new XDocument(new XElement("node", new XAttribute("selected", "true")));
            var nodes = _nodeParser.ParseNodes(xDocument);
            Assert.AreEqual(true, nodes.First().Selected);
        }
    }
}
