using System;
using System.Threading;
using System.Xml.Linq;
using Testura.Android.Device.Ui.Server;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Ui.Nodes
{
    public class ScreenDumper : IScreenDumper
    {
        private readonly IUiAutomatorServer _server;
        private readonly int _cooldownBetweenDump;
        private DateTime _lastDump;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenDumper"/> class.
        /// </summary>
        /// <param name="server">The ui dump server</param>
        /// <param name="cooldownBetweenDumps">Cooldown between dumps in miliseconds</param>
        public ScreenDumper(IUiAutomatorServer server, int cooldownBetweenDumps)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            _server = server;
            _cooldownBetweenDump = cooldownBetweenDumps;
            _lastDump = DateTime.Now;
        }

        /// <summary>
        /// Dump the current screen of the android device/emulator
        /// </summary>
        /// <returns>An xmldocument contaning all information about the current android screen</returns>
        public XDocument DumpUi()
        {
            Cooldown();
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

        private void Cooldown()
        {
            var milisecondsSinceLastDump = (int) (DateTime.Now - _lastDump).TotalMilliseconds;
            if (milisecondsSinceLastDump < _cooldownBetweenDump)
            {
                Thread.Sleep(_cooldownBetweenDump - milisecondsSinceLastDump);
            }

            _lastDump = DateTime.Now;
        }
    }
}