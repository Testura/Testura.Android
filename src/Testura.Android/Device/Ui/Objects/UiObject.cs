using System;
using System.Collections.Generic;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Device.Ui.Objects
{
    /// <summary>
    /// Represent a single UI Object on the screen
    /// </summary>
    public class UiObject : BaseUiObject
    {
        private readonly InteractionService _interactionService;
        private readonly INodeFinderService _nodeFinderService;

        internal UiObject(InteractionService interactionService, INodeFinderService nodeFinderService, IList<By> bys)
            : base(bys)
        {
            _interactionService = interactionService;
            _nodeFinderService = nodeFinderService;
        }

        /// <summary>
        /// Tap in the center of a node.
        /// </summary>
        public void Tap()
        {
            Tap(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Tap in the center of a node.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public void Tap(TimeSpan timeout)
        {
            var node = _nodeFinderService.FindNode(Bys, timeout);
            _interactionService.Tap(node);
        }

        /// <summary>
        /// Input text into the node.
        /// </summary>
        /// <param name="text">The text to input into the nod.e</param>
        public void InputText(string text)
        {
            InputText(text, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Input text into the node.
        /// </summary>
        /// <param name="text">The text to input into the nod.e</param>
        /// <param name="timeout">Timeout</param>
        public void InputText(string text, TimeSpan timeout)
        {
            Tap(timeout);
            var tries = 0;
            while (!WaitUntil(n => n.Focused, TimeSpan.FromSeconds(1)) && tries < 3)
            {
                Tap(TimeSpan.FromSeconds(1));
                tries++;
            }

            _interactionService.InputText(text);
        }

        /// <summary>
        /// Get the node values.
        /// </summary>
        /// <returns>A node object with all values of this node.</returns>
        public Node Values()
        {
            return Values(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Get the node values.
        /// </summary>
        /// <param name="timeout">Timeout (default 2 seconds)</param>
        /// <returns>A node object with all values of this node.</returns>
        public Node Values(TimeSpan timeout)
        {
            return _nodeFinderService.FindNode(Bys, timeout);
        }

        /// <summary>
        /// Wait for node values to match.
        /// </summary>
        /// <param name="expectedValues">Expected values on the node.</param>
        /// <returns>True if node values match before timeout, otherwise false.</returns>
        public bool WaitUntil(Func<Node, bool> expectedValues)
        {
            return WaitUntil(expectedValues, TimeSpan.FromSeconds(20));
        }

        /// <summary>
        /// Wait for node values to match.
        /// </summary>
        /// <param name="expectedValues">Expected values on the node.</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>True if node values match before timeout, otherwise false.</returns>
        public bool WaitUntil(Func<Node, bool> expectedValues, TimeSpan timeout)
        {
            var startTime = DateTime.Now;
            while (true)
            {
                var currentValues = Values(timeout);

                if (currentValues != null && expectedValues.Invoke(currentValues))
                {
                    return true;
                }

                if ((DateTime.Now - startTime).TotalMilliseconds > timeout.TotalMilliseconds)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Find node(s) on the screen.
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>All found node(s)</returns>
        protected override IList<Node> TryFindNode(TimeSpan timeout)
        {
            return new List<Node> { _nodeFinderService.FindNode(Bys, timeout) };
        }
    }
}