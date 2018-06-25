using System;
using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Services.Ui
{
    /// <summary>
    /// Defines a method to find nodes on the screen
    /// </summary>
    public interface INodeFinderService
    {
        /// <summary>
        /// Find a node on the screen
        /// </summary>
        /// <param name="withs">Find node with</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>Returns found node</returns>
        /// <exception cref="UiNodeNotFoundException">If we timeout and can't find the node</exception>
        Node FindNode(IList<With> withs, TimeSpan timeout);

        /// <summary>
        /// Find multiple nodes on the screen
        /// </summary>
        /// <param name="withs">Find node with</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>Returns found node</returns>
        /// <exception cref="UiNodeNotFoundException">If we timeout and can't find any nodes</exception>
        IList<Node> FindNodes(IList<With> withs, TimeSpan timeout);

        /// <summary>
        /// Get all nodes on the screen
        /// </summary>
        /// <returns>A list of all nodes on the screen</returns>
        IList<Node> AllNodes();
    }
}
