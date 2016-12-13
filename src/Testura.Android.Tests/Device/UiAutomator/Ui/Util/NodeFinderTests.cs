using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Tests.Device.UiAutomator.Ui.Util
{
    [TestFixture]
    public class NodeFinderTests
    {
        private NodeFinder _nodeFinder;
        private IList<Node> _nodes;

        [SetUp]
        public void SetUp()
        {
            _nodes = new List<Node>
            {
                new Node(new XElement("node", new XAttribute("text", "Buss")), null),
                new Node(new XElement("node", new XAttribute("resource-id", "android:id/search_src_text")), null),
                new Node(new XElement("node", new XAttribute("content-desc", "Number_ListItem_Container")), null),
                new Node(new XElement("node", new XAttribute("text", "Buss"), new XAttribute("class", "android.widget.TextView")), new Node(new XElement("node", new XAttribute("class", "android.support.v7.app.ActionBar$Tab")), null)),
                new Node(new XElement("node", new XAttribute("content-desc", "Number_ListItem")), null),
                new Node(new XElement("node", new XAttribute("content-desc", "Number_ListItem")), null)
        };
            _nodeFinder = new NodeFinder();
        }


        [Test]
        public void NodeChecker_WhenGettingNodeThatExist_ShouldReturnNode()
        {
            var node = _nodeFinder.FindNodes(_nodes, With.Text("Buss"));
            Assert.IsNotNull(node);
        }


        [Test]
        public void NodeChecker_WhenGettingNodeWithResourceId_ShouldReturnNode()
        {
            var foundNode = _nodeFinder.FindNode(_nodes, With.ResourceId("android:id/search_src_text"));
            Assert.IsNotNull(foundNode);
        }

        [Test]
        public void NodeChecker_WhenGettingNodeWithContentDesc_ShouldReturnNode()
        {
            var foundNode = _nodeFinder.FindNode(_nodes, With.ContentDesc("Number_ListItem_Container"));
            Assert.IsNotNull(foundNode);
        }

        [Test]
        public void NodeChecker_WhenGettingNodeWithLamba_ShouldReturnNode()
        {
            var foundNode = _nodeFinder.FindNode(_nodes, With.Lambda(node =>
                node.Text == "Buss" &&
                node.Class == "android.widget.TextView" &&
                node.Parent.Class == "android.support.v7.app.ActionBar$Tab"));
            Assert.IsNotNull(foundNode);
        }

        [Test]
        public void NodeChecker_WhenGettingMultipleNodes_ShouldReturnAllMatchingNodes()
        {
            var nodes = _nodeFinder.FindNodes(_nodes, With.Lambda(n => n.ContentDesc == "Number_ListItem"));
            Assert.AreEqual(2, nodes.Count);
        }
    }
}
