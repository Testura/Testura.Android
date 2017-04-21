using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util.Walker.Cases
{
    public class StopCase
    {
        /// <summary>
        /// Gets or sets the with for the special case node
        /// </summary>
        public IList<With> Withs { get; set; }

        /// <summary>
        /// Gets or sets the func that should be executed when we find the special case node. Returns
        /// true if we stop the walking
        /// </summary>
        public Func<IAndroidDevice, bool> Case { get; set; }

        /// <summary>
        /// Check if selected node match this tap case
        /// </summary>
        /// <param name="nodes">All nodes on screen</param>
        /// <returns>True if we match, false otherwise</returns>
        public bool IsMatching(IList<Node> nodes)
        {
            return nodes.Any(node => Withs.All(with => with.NodeSearch(node)));
        }
    }
}
