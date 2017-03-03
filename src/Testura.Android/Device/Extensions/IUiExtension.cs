using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes.Data;

namespace Testura.Android.Device.Extensions
{
    public interface IUiExtension
    {
        /// <summary>
        /// Check nodes on the screen for abnormalities.
        /// </summary>
        /// <param name="nodes">A list with nodes on the screen</param>
        /// <param name="device">Current android device</param>
        /// <returns>True if we should reset the regular find node time, leave it untouched if false</returns>
        bool CheckNodes(IList<Node> nodes, IAndroidDevice device);
    }
}
