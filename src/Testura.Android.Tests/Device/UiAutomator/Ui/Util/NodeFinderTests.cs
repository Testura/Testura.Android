using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

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
                new Node(new XElement("node", new XAttribute("package", "myPackage")), null),
                new Node(new XElement("node", new XAttribute("index", 1)), null),
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
        public void NodeChecker_WhenGettingNodeThatDontExist_ShouldThrowException()
        {
            Assert.Throws<UiNodeNotFoundException>(() => _nodeFinder.FindNodes(_nodes, new[] { With.Text("fdsfsdf") }));
        }

        [Test]
        public void NodeChecker_WhenGettingNodeWithText_ShouldReturnNode()
        {
            var foundNode = _nodeFinder.FindNode(_nodes, new[] {With.Text("Buss")});
            Assert.IsNotNull(foundNode);
            Assert.AreEqual("Buss", foundNode.Text);
        }


        [Test]
        public void NodeChecker_WhenGettingNodeWithResourceId_ShouldReturnNode()
        {
            var foundNode = _nodeFinder.FindNode(_nodes, new[] {With.ResourceId("android:id/search_src_text")});
            Assert.IsNotNull(foundNode);
            Assert.AreEqual("android:id/search_src_text", foundNode.ResourceId);
        }

        [Test]
        public void NodeChecker_WhenGettingNodeWithContentDesc_ShouldReturnNode()
        {
            var foundNode = _nodeFinder.FindNode(_nodes, new[] {With.ContentDesc("Number_ListItem_Container")});
            Assert.IsNotNull(foundNode);
            Assert.AreEqual("Number_ListItem_Container", foundNode.ContentDesc);
        }

        [Test]
        public void NodeChecker_WhenGettingNodeWithClass_ShouldReturnNode()
        {
            var foundNode = _nodeFinder.FindNode(_nodes, new[] {With.Class("android.widget.TextView")});
            Assert.IsNotNull(foundNode);
            Assert.AreEqual("android.widget.TextView", foundNode.Class);
        }

        [Test]
        public void NodeChecker_WhenGettingNodeWithIndex_ShouldReturnNode()
        {
            var foundNode = _nodeFinder.FindNode(_nodes, new[] {With.Index(1)});
            Assert.IsNotNull(foundNode);
            Assert.AreEqual("1", foundNode.Index);
        }

        [Test]
        public void NodeChecker_WhenGettingNodeWithPackage_ShouldReturnNode()
        {
            var foundNode = _nodeFinder.FindNode(_nodes, new[] {With.Package("myPackage")});
            Assert.IsNotNull(foundNode);
            Assert.AreEqual("myPackage", foundNode.Package);
        }

        [Test]
        public void NodeChecker_WhenGettingNodeWithLamba_ShouldReturnNode()
        {
            var foundNode = _nodeFinder.FindNode(_nodes, new[] {With.Lambda(node =>
                node.Text == "Buss" &&
                node.Class == "android.widget.TextView" &&
                node.Parent.Class == "android.support.v7.app.ActionBar$Tab")});
            Assert.IsNotNull(foundNode);
        }

        [Test]
        public void NodeChecker_WhenGettingMultipleNodes_ShouldReturnAllMatchingNodes()
        {
            var nodes = _nodeFinder.FindNodes(_nodes, new[] {With.Lambda(n => n.ContentDesc == "Number_ListItem")});
            Assert.AreEqual(2, nodes.Count);
        }
    }
}
