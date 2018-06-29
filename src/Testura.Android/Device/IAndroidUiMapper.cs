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
        /// <param name="by">How we should map ui object. Node should match all provided bys</param>
        /// <returns>The mapped ui object</returns>
        UiObject MapUiObject(params By[] by);

        /// <summary>
        /// Create a new ui object that maps to multiple nodes with same matching properties
        /// </summary>
        /// <param name="by">How we should map ui object. Node should match all provided bys</param>
        /// <returns>The mapped ui object</returns>
        UiObjects MapUiObjects(params By[] by);
    }
}
