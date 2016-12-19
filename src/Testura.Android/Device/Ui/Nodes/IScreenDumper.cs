using System.Xml.Linq;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using Testura.Android.Util.Exceptions;
#pragma warning restore IDE0005 // Using directive is unnecessary.

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
        /// Start the UI server
        /// </summary>
        /// <exception cref="UiAutomatorServerException">Thrown if we can't server</exception>
        void StartUiServer();

        /// <summary>
        /// Stop the UI server
        /// </summary>
        void StopUiServer();
    }
}