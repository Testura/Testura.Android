using System;
using System.Xml;
using System.Xml.Linq;
using Testura.Android.Device.Ui.Server;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Ui.Nodes
{
    public class ScreenDumper : IScreenDumper
    {
        private const int DumpTries = 3;

        private readonly IUiAutomatorServer _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenDumper"/> class.
        /// </summary>
        /// <param name="server">The ui dump server</param>
        public ScreenDumper(IUiAutomatorServer server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            _server = server;
        }

        /// <summary>
        /// Start the UI server
        /// </summary>
        /// <exception cref="UiAutomatorServerException">Thrown if we can't server</exception>
        public void StartUiServer()
        {
            _server.Start();
        }

        /// <summary>
        /// Stop the UI server
        /// </summary>
        public void StopUiServer()
        {
            _server.Stop();
        }

        /// <summary>
        /// Dump the current screen of the android device/emulator
        /// </summary>
        /// <returns>An xmldocument contaning all information about the current android screen</returns>
        public XDocument DumpUi()
        {
            var dump = GetDump();
            try
            {
                return XDocument.Parse(dump);
            }
            catch (XmlException ex)
            {
                throw new UiNodeNotFoundException("Could not parse nodes from dump", ex);
            }
        }

        private string GetDump()
        {
            if (!_server.Alive(2))
            {
                _server.Start();
            }

            return _server.DumpUi();
        }
    }
}