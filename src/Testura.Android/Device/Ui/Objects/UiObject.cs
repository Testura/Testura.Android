using System;
using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Device.Ui.Objects
{
    /// <summary>
    /// A wrapper for a single node on the screen
    /// </summary>
    public class UiObject : BaseUiObject
    {
        internal UiObject(IAndroidDevice device, params With[] with)
            : base(device, with)
        {
        }

        /// <summary>
        /// Tap in the center of a node
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        public void Tap(int timeout = 20)
        {
            var node = Device.Ui.FindNode(timeout, With);
            Device.Interaction.Tap(node);
        }

        /// <summary>
        /// Input text to the node
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="timeout">Timeout in seconds</param>
        public void InputText(string text, int timeout = 20)
        {
            Tap(timeout);
            var tries = 0;
            while (!WaitForValue(n => n.Focused, 1) && tries < 3)
            {
                Tap(1);
                tries++;
            }

            Device.Interaction.InputText(text);
        }

        /// <summary>
        /// Get the node values
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>A node object with all values of this node</returns>
        public Node Values(int timeout = 2)
        {
            return Device.Ui.FindNode(timeout, With);
        }

        /// <summary>
        /// Wait for node values to match
        /// </summary>
        /// <param name="expectedValues">Expected values on the node</param>
        /// <param name="timeout">Timeout in seconds</param>
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

                if ((DateTime.Now - startTime).Seconds > timeout)
                {
                    return false;
                }
            }
        }

        protected override IList<Node> TryFindNode(int timeout)
        {
            return new List<Node> { Device.Ui.FindNode(timeout, With) };
        }
    }
}