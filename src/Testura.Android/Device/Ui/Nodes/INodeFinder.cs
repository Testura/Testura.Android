using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Device.Ui.Nodes
{
    public interface INodeFinder
    {
        /// <summary>
        /// Search through a list of nodes and return the first node that match the search criteria.
        /// </summary>
        /// <param name="nodes">A list with nodes to search through</param>
        /// <param name="with">One ore many search criteria</param>
        /// <returns>The first node we find that match the search criteria</returns>
        Node FindNode(IList<Node> nodes, params With[] with);

        /// <summary>
        /// Search through a list of nodes and return all nodes that match the search criteria.
        /// </summary>
        /// <param name="nodes">A list with nodes to search through</param>
        /// <param name="with">One ore many search criteria</param>
        /// <returns>All nodes we find that match the search criteria</returns>
        IList<Node> FindNodes(IList<Node> nodes, params With[] with);
    }
}
