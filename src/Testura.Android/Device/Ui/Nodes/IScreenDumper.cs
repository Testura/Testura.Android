using System.Xml.Linq;

namespace Testura.Android.Device.Ui.Nodes
{
    public interface IScreenDumper
    {
        /// <summary>
        /// Dump the current screen of the android device/emulator
        /// </summary>
        /// <returns>An xmldocument contaning all information about the current android screen</returns>
        XDocument DumpUi();

        /// <summary>
        /// Stop the UI server
        /// </summary>
        void StopUiServer();
    }
}