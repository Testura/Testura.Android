using System.Collections.Generic;
using Testura.Android.Device.UiAutomator.Ui.Search;

namespace Testura.Android.Device.UiAutomator.Ui
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
        /// Click in the center of a node
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>The ui object</returns>
        public BaseUiObject Click(int timeout = 20)
        {
            var node = Device.Ui.FindNode(timeout, With);
            Device.Interaction.Click(node);
            return this;
        }

        /// <summary>
        /// Send text to the node
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>The ui object</returns>
        public BaseUiObject SendKeys(string text, int timeout = 20)
        {
            Click(timeout);
            Device.Interaction.SendKeys(text);
            return this;
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

        protected override IList<Node> TryFindNode(int timeout)
        {
            return new List<Node> { Device.Ui.FindNode(timeout, With) };
        }
    }
}
