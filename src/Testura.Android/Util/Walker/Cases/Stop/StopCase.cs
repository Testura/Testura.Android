using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util.Walker.Cases.Stop
{
    /// <summary>
    /// Provides the base class from which the classes that represent stop cases are derived. Stop cases make it possible
    /// to tell when we should stop an app walker run.
    /// </summary>
    public abstract class StopCase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopCase"/> class.
        /// </summary>
        /// <param name="wheres">A set of withs to find specific node.</param>
        protected StopCase(IList<Where> wheres)
        {
            Wheres = wheres;
        }

        /// <summary>
        /// Gets or sets the bys for the special case node
        /// </summary>
        public IList<Where> Wheres { get; set; }

        /// <summary>
        /// Check if selected node match this tap case
        /// </summary>
        /// <param name="nodes">All nodes on screen</param>
        /// <returns>True if we match, false otherwise</returns>
        public bool IsMatching(IList<Node> nodes)
        {
            return nodes.Any(node => Wheres.All(with => with.NodeMatch(node, null)));
        }

        /// <summary>
        /// Execute the case
        /// </summary>
        /// <param name="device">The current device</param>
        /// <returns>True if we should stop the app walker run, false otherwise</returns>
        public abstract bool Execute(IAndroidDevice device);
    }
}
