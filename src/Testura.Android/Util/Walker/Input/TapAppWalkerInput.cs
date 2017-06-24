using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Util.Walker.Cases.Tap;

namespace Testura.Android.Util.Walker.Input
{
    /// <summary>
    /// Provides functionality to handle input and tap cases in a app walker run.
    /// </summary>
    public class TapAppWalkerInput : IAppWalkerInput
    {
        private readonly bool _shouldOnlyTapClickAbleNodes;
        private readonly IEnumerable<TapCase> _tapCases;
        private readonly Random _rnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="TapAppWalkerInput"/> class.
        /// </summary>
        /// <param name="shouldOnlyTapClickAbleNodes"><c>true</c> if we should only click on nodes that are clickable, otherwise <c>false</c>.</param>
        public TapAppWalkerInput(bool shouldOnlyTapClickAbleNodes)
        {
            _tapCases = new List<TapCase>();
            _shouldOnlyTapClickAbleNodes = shouldOnlyTapClickAbleNodes;
            _rnd = new Random();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TapAppWalkerInput"/> class.
        /// </summary>
        /// <param name="shouldOnlyTapClickAbleNodes"><c>true</c> if we should only click on nodes that are click able.</param>
        /// <param name="tapCases">A set of tap cases that tells us what to do for specific nodes.</param>
        public TapAppWalkerInput(bool shouldOnlyTapClickAbleNodes, IEnumerable<TapCase> tapCases)
            : this(shouldOnlyTapClickAbleNodes)
        {
            _tapCases = tapCases;
        }

        /// <summary>
        /// Perform app walker input
        /// </summary>
        /// <param name="device">The current device</param>
        /// <param name="nodes">All nodes on current screen</param>
        public void PerformInput(IAndroidDevice device, IList<Node> nodes)
        {
            if (_shouldOnlyTapClickAbleNodes)
            {
                nodes = nodes.Where(n => n.Clickable).ToList();
            }

            if (nodes.Count == 0)
            {
                device.Interaction.InputKeyEvent(KeyEvents.Back);
                return;
            }

            var selectedNode = nodes[_rnd.Next(0, nodes.Count)];
            var shouldStillTap = CheckTapCases(device, _tapCases, selectedNode);

            if (shouldStillTap)
            {
                device.Interaction.Tap(selectedNode);

                if (selectedNode.Focusable && selectedNode.LongClickable)
                {
                    device.Interaction.InputText("Hello");
                }
            }
        }

        /// <summary>
        /// Check if we have any matching tap case for this node and if so invoke it first.
        /// </summary>
        /// <param name="device">The current device.</param>
        /// <param name="tapCases">List of provided tap cases.</param>
        /// <param name="selectedNode">The currently selected node.</param>
        /// <returns>True if we still should tap the node, false otherwise.</returns>
        private bool CheckTapCases(IAndroidDevice device, IEnumerable<TapCase> tapCases, Node selectedNode)
        {
            var shouldStillTap = true;
            var matchingTapCase = tapCases.FirstOrDefault(t => t.IsMatching(selectedNode));

            if (matchingTapCase != null)
            {
                shouldStillTap = matchingTapCase.Execute(device, selectedNode);
            }

            return shouldStillTap;
        }
    }
}
