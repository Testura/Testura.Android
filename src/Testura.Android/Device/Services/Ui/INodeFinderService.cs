using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Services.Ui
{
    public interface INodeFinderService
    {
        /// <summary>
        /// Find a node on the screen
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="with">Find node with</param>
        /// <returns>Returns found node</returns>
        /// <exception cref="UiNodeNotFoundException">If we timeout and can't find the node</exception>
        Node FindNode(int timeout, params With[] with);

        /// <summary>
        /// Find multiple nodes on the screen
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="with">Find node with</param>
        /// <returns>Returns found node</returns>
        /// <exception cref="UiNodeNotFoundException">If we timeout and can't find any nodes</exception>
        IList<Node> FindNodes(int timeout, With[] with);

        IList<Node> AllNodes();
    }
}
