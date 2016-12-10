using System;
using System.Xml.Linq;
using Testura.Android.Device.UiAutomator.Server;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.UiAutomator.Ui.Util
{
    public class ScreenDumper : IScreenDumper
    {
        private readonly IUiAutomatorServer _server;

        public ScreenDumper(IUiAutomatorServer server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            _server = server;
        }

        /// <summary>
        /// Dump the current screen of the android device/emulator
        /// </summary>
        /// <returns>An xmldocument contaning all information about the current android screen</returns>
        public XDocument DumpUi()
        {
            var dump = _server.DumpUi();
            if (string.IsNullOrEmpty(dump))
            {
                if (_server.Alive(2))
                {
                    // If we still can ping the server it must mean
                    // that the instrumentation has stopped
                    // but the http server is still running.
                    _server.Stop();
                }

                _server.Start();

                dump = _server.DumpUi();
                if (string.IsNullOrEmpty(dump))
                {
                    throw new UiAutomatorServerException("Could not dump screen");
                }
            }

            return XDocument.Parse(dump);
        }
    }
}