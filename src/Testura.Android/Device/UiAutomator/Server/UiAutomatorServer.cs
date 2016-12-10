using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Device.UiAutomator.Server
{
    public class UiAutomatorServer : IUiAutomatorServer
    {
        public const int DevicePort = 9030;
        private const int Timeout = 5;

        private readonly string _adbPath;
        private readonly string _serial;
        private readonly int _localPort;
        private readonly ITerminal _terminal;
        private Process _currentServerProcess;

        public UiAutomatorServer(ITerminal terminal, int port, string adbPath, string serial)
        {
            if (terminal == null)
            {
                throw new ArgumentNullException(nameof(terminal));
            }

            _localPort = port;
            _adbPath = adbPath;
            _serial = serial;
            _terminal = terminal;
        }

        public UiAutomatorServer(ITerminal terminal, int port)
            : this(terminal, port, string.Empty, string.Empty)
        {
        }

        private string BaseUrl => $"http://localhost:{_localPort}";

        private string PingUrl => $"{BaseUrl}/ping";

        private string DumpUrl => $"{BaseUrl}/dump";

        private string StopUrl => $"{BaseUrl}/stop";

        /// <summary>
        /// Start the ui automator server on the android device
        /// </summary>
        public void Start()
        {
            ForwardPorts();
            _currentServerProcess =
                _terminal.StartTerminalProcess(
                    CreateAdbCommand(
                        $"shell am instrument -w -r -e debug false -e port {DevicePort} -e class com.testura.testuraandroidserver.Start#RunServer com.testura.testuraandroidserver.test/android.support.test.runner.AndroidJUnitRunner"));

            if (!Alive(5))
            {
                throw new UiAutomatorServerException("Could not start server");
            }
        }

        /// <summary>
        /// Stop the ui automator server on the android device
        /// </summary>
        public void Stop()
        {
            GetData(StopUrl);
            _currentServerProcess?.Close();
        }

        /// <summary>
        /// Check if the ui automator server is alive on the android device
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>True if server is a alive, false otherwise.</returns>
        public bool Alive(int timeout)
        {
            var time = DateTime.Now;
            while ((DateTime.Now - time).Seconds < timeout)
            {
                var result = Ping();
                if (result)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Send a screen dump request to the ui automator server on the android device.
        /// </summary>
        /// <returns>The screen content as a xml string</returns>
        public string DumpUi()
        {
            return GetData(DumpUrl);
        }

        /// <summary>
        /// Forward ports to the android device
        /// </summary>
        private void ForwardPorts()
        {
            _terminal.ExecuteCommand(CreateAdbCommand($"forward tcp:{_localPort} tcp:{DevicePort}"));
        }

        /// <summary>
        /// Ping the ui automator server on the andoid device
        /// </summary>
        /// <returns>True if we got response, false otherwise</returns>
        private bool Ping()
        {
            var response = GetData(PingUrl);
            return response == "Hello human.";
        }

        /// <summary>
        /// Send a get request to the servce
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>The response from the server, empty no response.</returns>
        private string GetData(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(Timeout);
                    var response = httpClient.GetAsync(url).Result;
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex) when (ex is AggregateException || ex is HttpRequestException || ex is WebException)
            {
                return string.Empty;
            }
        }

        private string CreateAdbCommand(string command)
        {
            string adbPath = string.IsNullOrEmpty(_adbPath) ? "adb" : _adbPath;
            string serial = string.IsNullOrEmpty(_serial) ? string.Empty : $"-s {_serial}";
            return $"{adbPath} {serial} {command}";
        }
    }
}
