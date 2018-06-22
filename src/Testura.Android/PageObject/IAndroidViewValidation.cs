using Testura.Android.Device;

namespace Testura.Android.PageObject
{
    public interface IAndroidViewValidation<T>
        where T : View
    {
        void Validate(IAndroidDevice device, T view);
    }
}
