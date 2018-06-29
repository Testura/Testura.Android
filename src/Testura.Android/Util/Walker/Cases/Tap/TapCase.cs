using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Walker.Input;

namespace Testura.Android.Util.Walker.Cases.Tap
{
    /// <summary>
    /// Provides the base class from which the classes that represent tap cases are derived. Tap cases
    /// make it possible to handle specific nodes in <see cref="TapAppWalkerInput"/>
    /// </summary>
    public abstract class TapCase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TapCase"/> class.
        /// </summary>
        /// <param name="bys">A set of special case withs</param>
        protected TapCase(IList<By> bys)
        {
            Bys = bys;
        }

        /// <summary>
        /// Gets or sets the with for the special case node
        /// </summary>
        public IList<By> Bys { get; set; }

        /// <summary>
        /// Check if selected node match this tap case
        /// </summary>
        /// <param name="node">Selected node</param>
        /// <returns>True if we match, false otherwise</returns>
        public bool IsMatching(Node node)
        {
            return Bys.All(by => by.NodeSearch(node));
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
