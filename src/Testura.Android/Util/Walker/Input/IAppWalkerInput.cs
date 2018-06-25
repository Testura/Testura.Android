using System.Collections.Generic;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;

namespace Testura.Android.Util.Walker.Input
{
    /// <summary>
    /// Defines methods to perform input when app walking.
    /// </summary>
    public interface IAppWalkerInput
    {
        /// <summary>
        /// Perform app walker input
        /// </summary>
        /// <param name="device">The current device</param>
        /// <param name="nodes">All nodes on current screen</param>
        void PerformInput(IAndroidDevice device, IList<Node> nodes);
    }
}
