using System.Xml.Linq;

namespace Testura.Android.Device.UiAutomator.Ui.Util
{
    public interface IScreenDumper
    {
       /// <summary>
       /// Dump the current screen of the android device/emulator
       /// </summary>
       /// <returns>An xmldocument contaning all information about the current android screen</returns>
       XDocument DumpUi();
    }
}