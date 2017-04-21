using System.Collections.Generic;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;

namespace Testura.Android.Util.Walker.Input
{
    public interface IAppWalkerInput
    {
        /// <summary>
        /// Peform app walker input
        /// </summary>
        /// <param name="device">The current device</param>
        /// <param name="nodes">All nodes on current screen</param>
        void PerformInput(IAndroidDevice device, IList<Node> nodes);
    }
}
