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
        /// Map an UI node that match search criteria to an Ui object
        /// </summary>
        /// <param name="with">Find node with</param>
        /// <returns>The mapped ui object</returns>
        UiObject MapUiNode(params With[] with);

        /// <summary>
        /// Map multiple UI nodes that match the same search criteria to a single UI object.
        /// </summary>
        /// <param name="with">Find nodes with</param>
        /// <returns>The mapped ui object</returns>
        UiObjects MapUiNodes(params With[] with);
    }
}
