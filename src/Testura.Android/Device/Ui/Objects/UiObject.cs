using System;
using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Device.Ui.Objects
{
    /// <summary>
    /// Represent a single UI Object on the screen
    /// </summary>
    public class UiObject : BaseUiObject
    {
        internal UiObject(IAndroidDevice device, params With[] withs)
            : base(device, withs)
        {
        }

        /// <summary>
        /// Tap in the center of a node.
        /// </summary>
        /// <param name="timeout">Timeout in seconds.</param>
        public void Tap(int timeout = 20)
        {
            var node = Device.Ui.FindNode(timeout, Withs);
            Device.Interaction.Tap(node);
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

            Device.Interaction.InputText(text);
        }

        /// <summary>
        /// Get the node values.
        /// </summary>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns>A node object with all values of this node.</returns>
        public Node Values(int timeout = 2)
        {
            return Device.Ui.FindNode(timeout, Withs);
        }

        /// <summary>
        /// Get the node values from the latest cached dump.
        /// </summary>
        /// <returns>A node object with all values of this node.</returns>
        public Node ValuesFromCache()
        {
            return Device.Ui.FindNodeFromCache(Withs);
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
            if (timeout == -1)
            {
                return new List<Node> { Device.Ui.FindNodeFromCache(Withs) };
            }

            return new List<Node> { Device.Ui.FindNode(timeout, Withs) };
        }
    }
}