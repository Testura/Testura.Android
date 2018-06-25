using Testura.Android.Device;

namespace Testura.Android.PageObject
{
    /// <summary>
    /// Defines and interface to validate one or multiple things on a view
    /// </summary>
    /// <typeparam name="T">View to validate</typeparam>
    public interface IAndroidViewValidation<in T>
        where T : View
    {
        /// <summary>
        /// Validate information on a specific view
        /// </summary>
        /// <param name="device">Current android device</param>
        /// <param name="view">View to validate</param>
        void Validate(IAndroidDevice device, T view);
    }
}
