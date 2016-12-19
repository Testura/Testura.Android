using System;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.Http;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Device.Ui.Server
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
        /// <exception cref="UiAutomatorServerException">Thrown if we can't server</exception>
        public void Start()
        {
            DeviceLogger.Log("Starting server..");
            ForwardPorts();
            if (_currentServerProcess == null || _currentServerProcess.HasExited)
            {
                DeviceLogger.Log("Starting instrumental");
                _currentServerProcess =
                    _terminal.StartTerminalProcess(
                        CreateAdbCommand(
                            $"shell am instrument -w -r -e debug false -e port {DevicePort} -e class com.testura.testuraandroidserver.Start#RunServer com.testura.testuraandroidserver.test/android.support.test.runner.AndroidJUnitRunner"));
            }
            else
            {
                DeviceLogger.Log("Server already started");
            }

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
            DeviceLogger.Log("Stopping server");
            GetData(StopUrl);
            if (_currentServerProcess != null)
            {
                KillProcessAndChildrens(_currentServerProcess.Id);
            }
        }

        /// <summary>
        /// Check if the ui automator server is alive on the android device
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>True if server is a alive, false otherwise.</returns>
        public bool Alive(int timeout)
        {
            DeviceLogger.Log("Checking if server is alive");
            var time = DateTime.Now;
            while ((DateTime.Now - time).Seconds < timeout)
            {
                var result = Ping();
                if (result)
                {
                    DeviceLogger.Log("Server is alive!");
                    return true;
                }
            }

            DeviceLogger.Log("Server is not alive!");
            return false;
        }

        /// <summary>
        /// Send a screen dump request to the ui automator server on the android device.
        /// </summary>
        /// <returns>The screen content as a xml string</returns>
        public string DumpUi()
        {
            DeviceLogger.Log("Dumping ui");
            var dump = GetData(DumpUrl);
            if (string.IsNullOrEmpty(dump))
            {
                DeviceLogger.Log("Failed to dump!");
                return string.Empty;
            }

            DeviceLogger.Log("Dump was successful");
            return dump;
        }

        /// <summary>
        /// Forward ports to the android device
        /// </summary>
        private void ForwardPorts()
        {
            DeviceLogger.Log("Forwarding ports");
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
            var adbPath = string.IsNullOrEmpty(_adbPath) ? "adb" : _adbPath;
            var serial = string.IsNullOrEmpty(_serial) ? string.Empty : $"-s {_serial}";
            return $"{adbPath} {serial} {command}";
        }

        private void KillProcessAndChildrens(int pid)
        {
            var processSearcher = new ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            var processCollection = processSearcher.Get();

            try
            {
                var proc = Process.GetProcessById(pid);
                if (!proc.HasExited)
                {
                    proc.Kill();
                }
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }

            if (processCollection != null)
            {
                foreach (var mo in processCollection)
                {
                    KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"]));
                }
            }
        }
    }
}
