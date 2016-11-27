using System.Xml.Linq;

namespace Testura.Android.UiAutomator
{
    public interface IScreenDumper
    {
       XDocument DumpUi();
    }
}