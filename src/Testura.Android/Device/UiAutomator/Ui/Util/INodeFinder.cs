using System.Collections.Generic;
using Testura.Android.Device.UiAutomator.Ui.Search;

namespace Testura.Android.Device.UiAutomator.Ui.Util
{
    public interface INodeFinder
    {
        /// <summary>
        /// Search through a list of nodes and return the first node that match the search criterias.
        /// </summary>
        /// <param name="nodes">A list with nodes to search through</param>
        /// <param name="with">One ore many search criterias</param>
        /// <returns>The first node we find that match the search criterias</returns>
        Node FindNode(IList<Node> nodes, params With[] with);

        /// <summary>
        /// Search through a list of nodes and return all nodes that match the search criterias.
        /// </summary>
        /// <param name="nodes">A list with nodes to search through</param>
        /// <param name="with">One ore many search criterias</param>
        /// <returns>All nodes we find that match the search criterias</returns>
        IList<Node> FindNodes(IList<Node> nodes, params With[] with);
    }
}
