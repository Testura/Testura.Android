using System;
using System.Collections.Generic;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util.Walker.Cases.Tap
{
    /// <summary>
    /// Provides functionality to handle specific nodes by invoke a func.
    /// </summary>
    public class FuncTapCase : TapCase
    {
        private readonly Func<AndroidDevice, Node, bool> _case;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncTapCase"/> class.
        /// </summary>
        /// <param name="withs">Withs to find the node in our case</param>
        /// <param name="case">A func which take the current device, currently selected node and returns if we still should tap on node.</param>
        public FuncTapCase(IList<With> withs, Func<AndroidDevice, Node, bool> @case)
            : base(withs)
        {
            _case = @case;
        }

        /// <summary>
        /// Execute the case
        /// </summary>
        /// <param name="devie">The current device</param>
        /// <param name="node">The currently selected node</param>
        /// <returns>True if we should still tap on node, false otherwise</returns>
        public override bool Execute(AndroidDevice devie, Node node)
        {
            return _case.Invoke(devie, node);
        }
    }
}
