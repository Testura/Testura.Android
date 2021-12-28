﻿using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Device.Ui.Nodes
{
    /// <summary>
    /// Define methods to find node(s) in a node list.
    /// </summary>
    public interface INodeFinder
    {
        /// <summary>
        /// Search through a list of nodes and return the first node that match the search criteria.
        /// </summary>
        /// <param name="nodes">A list withs nodes to search through.</param>
        /// <param name="wheres">One ore many search criterias.</param>
        /// <param name="wildcard">Wildcard value</param>
        /// <returns>The first node we find that match the search criteria.</returns>
        Node FindNode(IList<Node> nodes, IList<Where> wheres, string wildcard = null);

        /// <summary>
        /// Search through a list of nodes and return all nodes that match the search criteria.
        /// </summary>
        /// <param name="nodes">A list withs nodes to search through.</param>
        /// <param name="wheres">One ore many search criteria.</param>
        /// <param name="wildcard">wildcard value</param>
        /// <returns>All nodes we find that match the search criteria.</returns>
        IList<Node> FindNodes(IList<Node> nodes, IList<Where> wheres, string wildcard = null);
    }
}
