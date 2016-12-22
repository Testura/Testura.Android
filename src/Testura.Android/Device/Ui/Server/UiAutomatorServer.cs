using System;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.Http;
using Medallion.Shell;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Device.Ui.Server
{
    public class UiAutomatorServer : IUiAutomatorServer
    {
        public const int DevicePort = 9030;
        private const int Timeout = 5;

        private readonly int _localPort;
        private readonly ITerminal _terminal;
        private Command _currentServerProcess;

        public UiAutomatorServer(ITerminal terminal, int port)
        {
            if (terminal == null)
            {
                throw new ArgumentNullException(nameof(terminal));
            }

            _localPort = port;
            _terminal = terminal;
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
            if (_currentServerProcess == null || _currentServerProcess.Process.HasExited)
            {
                DeviceLogger.Log("Starting instrumental");
                _currentServerProcess = _terminal.StartAdbProcess("shell", "am", "instrument", "-w", "-r", "-e", "debug", "false", "-e", "port",
                    DevicePort.ToString(),
                    "-e", "class",
                    "com.testura.testuraandroidserver.Start#RunServer com.testura.testuraandroidserver.test/android.support.test.runner.AndroidJUnitRunner");
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
            KillProcessAndChildrens(_currentServerProcess.Process.Id);
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
            var dump = GetData(DumpUrl);
            if (string.IsNullOrEmpty(dump))
            {
                DeviceLogger.Log("Failed to dump!");
                return string.Empty;
            }

            return dump;
        }

        /// <summary>
        /// Forward ports to the android device
        /// </summary>
        private void ForwardPorts()
        {
            DeviceLogger.Log("Forwarding ports");
            _terminal.ExecuteAdbCommand("forward", $"tcp:{_localPort}", $"tcp:{DevicePort}");
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
