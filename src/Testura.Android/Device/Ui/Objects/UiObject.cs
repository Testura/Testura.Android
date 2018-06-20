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

        internal UiObject(InteractionService interactionService, INodeFinderService nodeFinderService, params With[] withs)
            : base(withs)
        {
            _interactionService = interactionService;
            _nodeFinderService = nodeFinderService;
        }

        /// <summary>
        /// Tap in the center of a node.
        /// </summary>
        /// <param name="timeout">Timeout in seconds.</param>
        public void Tap(int timeout = 20)
        {
            var node = _nodeFinderService.FindNode(timeout, Withs);
            _interactionService.Tap(node);
        }

        /// <summary>
        /// Input text into the node.
        /// </summary>
        /// <param name="text">The text to input into the nod.e</param>
        /// <param name="timeout">Timeout in second.</param>
        public void InputText(string text, int timeout = 20)
        {
            Tap(timeout);
            var tries = 0;
            while (!WaitForValue(n => n.Focused, 1) && tries < 3)
            {
                Tap(1);
                tries++;
            }

            _interactionService.InputText(text);
        }

        /// <summary>
        /// Get the node values.
        /// </summary>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns>A node object with all values of this node.</returns>
        public Node Values(int timeout = 2)
        {
            return _nodeFinderService.FindNode(timeout, Withs);
        }

        /// <summary>
        /// Wait for node values to match.
        /// </summary>
        /// <param name="expectedValues">Expected values on the node.</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns>True if node values match before timeout, otherwise false.</returns>
        public bool WaitForValue(Func<Node, bool> expectedValues, int timeout = 20)
        {
            var startTime = DateTime.Now;
            while (true)
            {
                var currentValues = Values(timeout);

                if (currentValues != null && expectedValues.Invoke(currentValues))
                {
                    return true;
                }

                if ((DateTime.Now - startTime).TotalSeconds > timeout)
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
        protected override IList<Node> TryFindNode(int timeout)
        {
            return new List<Node> { _nodeFinderService.FindNode(timeout, Withs) };
        }
    }
}