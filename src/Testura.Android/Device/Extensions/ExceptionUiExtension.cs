using System.Collections.Generic;
using Testura.Android.Device.UiAutomator.Ui;
using Testura.Android.Device.UiAutomator.Ui.Search;
using Testura.Android.Device.UiAutomator.Ui.Util;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Extensions
{
    /// <summary>
    /// Example class for an ui extension
    /// </summary>
    public class ExceptionUiExtension : IUiExtension
    {
        private readonly NodeFinder _nodeFinder;

        public ExceptionUiExtension()
        {
            _nodeFinder = new NodeFinder();
        }

        /// <summary>
        /// Check nodes for exception window.
        /// </summary>
        /// <param name="nodes">A list with nodes on the screen</param>
        /// <param name="device">Current android device</param>
        public void CheckNodes(IList<Node> nodes, IAndroidDevice device)
        {
            try
            {
                var node = _nodeFinder.FindNode(nodes, With.Class("test.."));
                device.Interaction.Click(node);
            }
            catch (UiNodeNotFoundException ex)
            {
            }
        }
    }
}
