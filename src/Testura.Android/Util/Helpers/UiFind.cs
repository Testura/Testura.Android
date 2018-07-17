using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util.Helpers
{
    /// <summary>
    /// Provides functionality to find nodes
    /// </summary>
    public static class UiFind
    {
        /// <summary>
        /// Find the closest node(based on another node) that matches specific where(s)
        /// </summary>
        /// <param name="startNode">The start node and position where we should start to search</param>
        /// <param name="wheres">"Where(s)" that the close node should match</param>
        /// <returns>The closest node, null if we can't find a matching node</returns>
        /// <example>This example show how to call the method with another node </example>
        /// <code>
        /// var node = uiObject.Values()
        /// UiFind.ClosestNode(Where.Text("test"), node)
        /// </code>
        public static Node ClosestNode(Node startNode, params Where[] wheres)
        {
            return FindNode(wheres, startNode, new List<Node>(), 0, null)?.Node;
        }

        private static FoundNode FindNode(Where[] wheres, Node current, ICollection<Node> visitedNodes, int distance, FoundNode foundNode)
        {
            visitedNodes.Add(current);

            if (wheres.All(w => w.NodeMatch.Invoke(current, null)))
            {
                if (foundNode == null || distance < foundNode.Distance)
                {
                    foundNode = new FoundNode(distance, current);
                }
            }

            foreach (var currentChild in current.Children)
            {
                var found = FindNode(wheres, currentChild, visitedNodes, distance + 1, foundNode);
                if (NodeOk(foundNode, found))
                {
                    foundNode = found;
                }
            }

            if (current.Parent != null && !visitedNodes.Contains(current.Parent))
            {
                var found = FindNode(wheres, current.Parent, visitedNodes, distance + 1, foundNode);
                if (NodeOk(foundNode, found))
                {
                    foundNode = found;
                }
            }

            return foundNode;
        }

        private static bool NodeOk(FoundNode foundNode, FoundNode found)
        {
            return found != null && (foundNode == null || found.Distance < foundNode.Distance);
        }

        private class FoundNode
        {
            public FoundNode(int distance, Node node)
            {
                Distance = distance;
                Node = node;
            }

            public int Distance { get; }

            public Node Node { get; }
        }
    }
}
