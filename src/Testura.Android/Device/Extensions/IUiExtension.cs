using System.Collections.Generic;
using Testura.Android.Device.UiAutomator.Ui;

namespace Testura.Android.Device.Extensions
{
    public interface IUiExtension
    {
        /// <summary>
        /// Check nodes on the screen for abnormalities.
        /// </summary>
        /// <param name="nodes">A list with nodes on the screen</param>
        /// <param name="device">Current android device</param>
        void CheckNodes(IList<Node> nodes, IAndroidDevice device);
    }
}
