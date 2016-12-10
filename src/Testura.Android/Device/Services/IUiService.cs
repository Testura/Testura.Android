using System.Collections.Generic;
using Testura.Android.Device.Extensions;
using Testura.Android.Device.UiAutomator.Ui;
using Testura.Android.Device.UiAutomator.Ui.Search;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using Testura.Android.Util.Exceptions;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace Testura.Android.Device.Services
{
    public interface IUiService
    {
        /// <summary>
        /// Gets a list of ui extensions
        /// </summary>
        IList<IUiExtension> Extensions { get; }

        /// <summary>
        /// Find a node on the screen
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="with">Find node with</param>
        /// <returns>Returns found node</returns>
        /// <exception cref="UiNodeNotFoundException">If we timeout and can't find the node</exception>
        Node FindNode(int timeout, params With[] with);

        /// <summary>
        /// Find multiple nodes on the screen
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="with">Find node with</param>
        /// <returns>Returns found node</returns>
        /// <exception cref="UiNodeNotFoundException">If we timeout and can't find any nodes</exception>
        IList<Node> FindNodes(int timeout, params With[] with);

        /// <summary>
        /// Create a new ui object that wraps around a node that match a specific search critiera
        /// </summary>
        /// <param name="with">Find node with</param>
        /// <returns>The mapped ui object</returns>
        UiObject CreateUiObject(params With[] with);

        /// <summary>
        /// Create a new ui object that wraps around multiple nodes that match a specific search critiera
        /// </summary>
        /// <param name="with">Find nodes with</param>
        /// <returns>The mapped ui object</returns>
        UiObjects CreateUiObjects(params With[] with);
    }
}