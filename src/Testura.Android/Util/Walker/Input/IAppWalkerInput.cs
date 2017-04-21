using System.Collections.Generic;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Util.Walker.Cases;

namespace Testura.Android.Util.Walker.Input
{
    public interface IAppWalkerInput
    {
        /// <summary>
        /// Peform app walker input
        /// </summary>
        /// <param name="device">The current device</param>
        /// <param name="tapCases">List of provided tap cases</param>
        /// <param name="nodes">All nodes on current screen</param>
        /// <param name="configuration">The current app walker configuration</param>
        void PerformInput(IAndroidDevice device, IEnumerable<TapCase> tapCases, IList<Node> nodes, AppWalkerConfiguration configuration);
    }
}
