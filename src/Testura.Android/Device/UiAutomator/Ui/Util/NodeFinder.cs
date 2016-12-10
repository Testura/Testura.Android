using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device.UiAutomator.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.UiAutomator.Ui.Util
{
    public class NodeFinder : INodeFinder
    {
        /// <summary>
        /// Search through a list of nodes and return the first node that match the search criterias.
        /// </summary>
        /// <param name="nodes">A list with nodes to search through</param>
        /// <param name="with">One ore many search criterias</param>
        /// <returns>The first node we find that match the search criterias</returns>
        public Node FindNode(IList<Node> nodes, params With[] with)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (with == null || with.Length == 0)
            {
                throw new ArgumentException("Argument is empty collection", nameof(with));
            }

            var foundNodes = FindNodes(nodes, with);
            return foundNodes.First();
        }

        /// <summary>
        /// Search through a list of nodes and return all nodes that match the search criterias.
        /// </summary>
        /// <param name="nodes">A list with nodes to search through</param>
        /// <param name="with">One ore many search criterias</param>
        /// <returns>All nodes we find that match the search criterias</returns>
        public IList<Node> FindNodes(IList<Node> nodes, params With[] with)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (with == null || with.Length == 0)
            {
                throw new ArgumentException("Argument is empty collection", nameof(with));
            }

            var approvedNodes = new List<Node>();

            foreach (var with1 in with)
            {
                var foundNodes = nodes.Where(with1.NodeSearche).ToList();
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
                throw new UiNodeNotFoundException(with);
            }

            return approvedNodes;
        }
    }
}
