using Testura.Android.UiAutomator;

namespace Testura.Android.Services
{
    public interface IUiService
    {
        Node GetNodeBy(By by, int timeout = 2);
    }
}