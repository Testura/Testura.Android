using System.Xml.Linq;
using NUnit.Framework;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Helpers;

namespace Testura.Android.Tests.Util.Helpers
{
    [TestFixture]
    public class UiFindTests
    {
        [Test]
        public void FindClosestNode_WhenSearchingForNodeThatExist_ShouldGetNode()
        {
            var root = CreateNode();
            var fondNode = UiFind.ClosestNode(root.Children[0], Where.Text("second_node_1"));
            Assert.IsNotNull(fondNode);
            Assert.AreEqual(root.Children[0].Children[1], fondNode);
        }

        [Test]
        public void FindClosestNode_WhenComplexSearchingForNodeThatExist_ShouldGetNode()
        {
            var root = CreateNode();
            var fondNode = UiFind.ClosestNode(root.Children[0], Where.Text("third_node_1_1_1"));
            Assert.IsNotNull(fondNode);
            Assert.AreEqual(root.Children[1].Children[1].Children[1], fondNode);
        }

        [Test]
        public void FindClosestNode_WhenSearchingForNodeThatDontExist_ShouldGetNull()
        {
            var root = CreateNode();
            var fondNode = UiFind.ClosestNode(root.Children[0], Where.Text("second_node_23"));
            Assert.IsNull(fondNode);
        }

        private Node CreateNode()
        {
            var rootNode = new Node(new XElement("node", new XAttribute("text", "root")), null);

            for (var n = 0; n < 2; n++)
            {
                var secondNode = new Node(new XElement("node", new XAttribute("text", $"node_{n}")), rootNode);

                for (var i = 0; i < 2; i++)
                {
                    secondNode.Children.Add(new Node(new XElement("node", new XAttribute("text", $"second_node_{i}")), secondNode));

                    for (var d = 0; d < 2; d++)
                    {
                        secondNode.Children[i].Children.Add(new Node(new XElement("node", new XAttribute("text", $"third_node_{n}_{i}_{d}")), secondNode.Children[i]));
                    }
                }

                rootNode.Children.Add(secondNode);
            }

            return rootNode;
        }
    }
}
