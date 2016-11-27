using Testura.Android.Services;

namespace Testura.Android.Device
{
    public interface IDeviceServices
    {
        IAdbService Adb { get; }

        IUiService Ui { get; }
    }
}
