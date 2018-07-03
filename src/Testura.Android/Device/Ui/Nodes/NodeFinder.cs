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
        /// <inheritdoc />
        public Node FindNode(IList<Node> nodes, IList<Where> wheres, string wildcard = null)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (wheres == null || !wheres.Any())
            {
                throw new ArgumentException("Argument is empty collection", nameof(wheres));
            }

            var foundNodes = FindNodes(nodes, wheres, wildcard);
            return foundNodes.First();
        }

        /// <inheritdoc />
        public IList<Node> FindNodes(IList<Node> nodes, IList<Where> wheres, string wildcard = null)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (wheres == null || !wheres.Any())
            {
                throw new ArgumentException("Argument is empty collection", nameof(wheres));
            }

            var approvedNodes = new List<Node>();

            foreach (var where in wheres)
            {
                var foundNodes = nodes.Where(n => where.NodeSearch(n, wildcard)).ToList();

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
                throw new UiNodeNotFoundException(wheres);
            }

            return approvedNodes;
        }
    }
}
