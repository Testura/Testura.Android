using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Device.Ui.Objects
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
            return TryFindNode(timeout);
        }

        protected override IList<Node> TryFindNode(int timeout)
        {
            return Device.Ui.FindNodes(timeout, With);
        }
    }
}
