using System;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Device
{
    /// <summary>
    /// Defines an interface to map ui elements to ui objects
    /// </summary>
    public interface IAndroidUiMapper
    {
        /// <summary>
        /// Lazy map an ui object based on a predicate.
        /// </summary>
        /// <param name="predicate">The func used to map the node(s). The func takes current node, provided wildcard and return true if the node map otherwise false.</param>
        /// <param name="customErrorMessage">Custom error message used later on if we can't find the mapped node</param>
        /// <returns>The mapped ui object</returns>
        UiObject MapUiObject(Func<Node, string, bool> predicate, string customErrorMessage = null);

        /// <summary>
        /// Lazy map an ui object based on a predicate.
        /// </summary>
        /// <param name="predicate">The func used to map the node(s). The func takes current node and return true if the node map otherwise false.</param>
        /// <param name="customErrorMessage">Custom error message used later on if we can't find the mapped node</param>
        /// <returns>The mapped ui object</returns>
        UiObject MapUiObject(Func<Node, bool> predicate, string customErrorMessage = null);

        /// <summary>
        /// Lazy map an ui object based on one or multiple <see cref="Where">Wher(s)</see>
        /// </summary>
        /// <param name="wheres">One or multiple "where(s)" that say how we should map the ui object.</param>
        /// <returns>The mapped ui object</returns>
        UiObject MapUiObject(params Where[] wheres);
    }
}
