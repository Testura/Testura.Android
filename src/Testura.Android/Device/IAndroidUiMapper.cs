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
        /// Create a new ui object that maps to a single node
        /// </summary>
        /// <param name="with">Map ui object with node that match this with</param>
        /// <returns>The mapped ui object</returns>
        UiObject MapUiNode(params With[] with);

        /// <summary>
        /// Create a new ui object that maps to multiple nodes with same properties
        /// </summary>
        /// <param name="with">Map ui object with nodes that match this with</param>
        /// <returns>The mapped ui object</returns>
        UiObjects MapUiNodes(params With[] with);
    }
}
