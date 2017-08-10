using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
#pragma warning disable 1591

namespace Testura.Android.Device.Ui.Objects
{
    /// <summary>
    /// Represent multiple UI Objects on the screen
    /// </summary>
    public class UiObjects : BaseUiObject
    {
        internal UiObjects(IAndroidDevice device, params With[] withs)
            : base(device, withs)
        {
        }

        /// <summary>
        /// Get a list of nodes that contain all values.
        /// </summary>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns>A list of nodes that contain all values.</returns>
        public IList<Node> Values(int timeout = 2)
        {
            return TryFindNode(timeout);
        }

        /// <summary>
        /// Get a list of nodes from the latest cached dump that contain all values.
        /// </summary>
        /// <returns>A list of nodes that contain all values.</returns>
        public IList<Node> ValuesFromCache()
        {
            return Device.Ui.FindNodesFromCache(Withs);
        }

        protected override IList<Node> TryFindNode(int timeout)
        {
            if (timeout == -1)
            {
                return Device.Ui.FindNodesFromCache(Withs);
            }

            return Device.Ui.FindNodes(timeout, Withs);
        }
    }
}
