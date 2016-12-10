using Testura.Android.Device;

namespace Testura.Android.PageObject
{
    /// <summary>
    /// This class represent a view in the application
    /// </summary>
    public abstract class View
    {
        protected View(IAndroidDevice device)
        {
            Device = device;
        }

        /// <summary>
        /// Gets or sets the current android device
        /// </summary>
        protected IAndroidDevice Device { get; set; }
    }
}
