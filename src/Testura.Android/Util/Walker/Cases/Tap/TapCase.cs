using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util.Walker.Cases.Tap
{
    public abstract class TapCase
    {
        protected TapCase(IList<With> withs)
        {
            Withs = withs;
        }

        /// <summary>
        /// Gets or sets the with for the special case node
        /// </summary>
        public IList<With> Withs { get; set; }

        /// <summary>
        /// Check if selected node match this tap case
        /// </summary>
        /// <param name="node">Selected node</param>
        /// <returns>True if we match, false otherwise</returns>
        public bool IsMatching(Node node)
        {
            return Withs.All(with => with.NodeSearch(node));
        }

        /// <summary>
        /// Execute the case
        /// </summary>
        /// <param name="devie">The current device</param>
        /// <param name="node">The currently selected node</param>
        /// <returns>True if we should still tap on node, false otherwise</returns>
        public abstract bool Execute(IAndroidDevice devie, Node node);
    }
}
