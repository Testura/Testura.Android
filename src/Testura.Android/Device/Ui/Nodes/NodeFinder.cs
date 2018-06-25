using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Ui.Nodes
{
    /// <summary>
    /// Provides functionality to find node(s) in a node list.
    /// </summary>
    public class NodeFinder : INodeFinder
    {
        /// <summary>
        /// Search through a list of nodes and return the first node that match the search criteria.
        /// </summary>
        /// <param name="nodes">A list withs nodes to search through.</param>
        /// <param name="withs">One ore many search criteria.</param>
        /// <returns>The first node we find that match the search criteria.</returns>
        public Node FindNode(IList<Node> nodes, IList<With> withs)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (withs == null || !withs.Any())
            {
                throw new ArgumentException("Argument is empty collection", nameof(withs));
            }

            var foundNodes = FindNodes(nodes, withs);
            return foundNodes.First();
        }

        /// <summary>
        /// Search through a list of nodes and return all nodes that match the search criteria.
        /// </summary>
        /// <param name="nodes">A list withs nodes to search through.</param>
        /// <param name="withs">One ore many search criteria.</param>
        /// <returns>All nodes we find that match the search criteria.</returns>
        public IList<Node> FindNodes(IList<Node> nodes, IList<With> withs)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (withs == null || !withs.Any())
            {
                throw new ArgumentException("Argument is empty collection", nameof(withs));
            }

            var approvedNodes = new List<Node>();

            foreach (var with in withs)
            {
                var foundNodes = nodes.Where(with.NodeSearch).ToList();

                if (!approvedNodes.Any())
                {
                    approvedNodes = foundNodes;
                }
                else
                {
                    approvedNodes = approvedNodes.Where(node => foundNodes.Contains(node)).ToList();
                }
            }

            if (!approvedNodes.Any())
            {
                throw new UiNodeNotFoundException(withs);
            }

            return approvedNodes;
        }
    }
}
