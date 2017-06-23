using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes.Data;

namespace Testura.Android.Device.Extensions
{
    /// <summary>
    /// Defines methods to extend the UI Service and make addtional node checks.
    /// </summary>
    public interface IUiExtension
    {
        /// <summary>
        /// Check nodes on the screen for abnormalities.
        /// </summary>
        /// <param name="nodes">A list with nodes on the screen.</param>
        /// <param name="device">Current android device.</param>
        /// <returns><c>true</c> if we should reset the regular find node timer, otherwise <c>false</c>/returns>
        bool CheckNodes(IList<Node> nodes, IAndroidDevice device);
    }
}
