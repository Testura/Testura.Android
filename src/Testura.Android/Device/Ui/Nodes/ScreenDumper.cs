using System;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Testura.Android.Device.Ui.Server;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Device.Ui.Nodes
{
    public class ScreenDumper : IScreenDumper
    {
        private readonly IUiAutomatorServer _server;
        private readonly int _dumpTries;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenDumper"/> class.
        /// </summary>
        /// <param name="server">The ui dump server</param>
        public ScreenDumper(IUiAutomatorServer server, int dumpTries)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            _server = server;
            _dumpTries = dumpTries;
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
        /// <returns>An xmldocument containing all information about the current android screen</returns>
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
            int tries = _dumpTries;
            while (true)
            {
                try
                {
                    if (!_server.Alive(2))
                    {
                        _server.Start();
                    }

                    return _server.DumpUi();
                }
                catch (UiAutomatorServerException ex)
                {
                    if (tries > 0)
                    {
                        DeviceLogger.Log($"Failed to dump UI, trying {tries} more times");
                        Thread.Sleep(1500);
                        tries--;
                        continue;
                    }

                    DeviceLogger.Log("Failed to dump UI!");
                    throw;
                }
            }
        }
    }
}