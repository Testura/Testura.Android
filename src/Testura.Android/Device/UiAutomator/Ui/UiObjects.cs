using System.Collections.Generic;
using Testura.Android.Device.UiAutomator.Ui.Search;

namespace Testura.Android.Device.UiAutomator.Ui
{
    /// <summary>
    /// A wrapper for multiple nodes on the screen
    /// </summary>
    public class UiObjects : BaseUiObject
    {
        internal UiObjects(IAndroidDevice device, params With[] with)
            : base(device, with)
        {
        }

        /// <summary>
        /// Get a list of nodes that contain all values
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>A list of nodes that contain all values</returns>
        public IList<Node> Values(int timeout = 2)
        {
            return TryFindNode(2);
        }

        protected override IList<Node> TryFindNode(int timeout)
        {
            return Device.Ui.FindNodes(timeout, With);
        }
    }
}
