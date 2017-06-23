using System;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Medallion.Shell;
using Testura.Android.Device.Configurations;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Device.Ui.Server
{
    /// <summary>
    /// Provides functionality to interact with the UI automator server
    /// </summary>
    public class UiAutomatorServer : IUiAutomatorServer
    {
        /// <summary>
        /// Get the device port.
        /// </summary>
        public const int DevicePort = 9008;

        /// <summary>
        /// Get the timeout in seconds.
        /// </summary>
        private const int Timeout = 5;

        private readonly int _localPort;
        private readonly ITerminal _terminal;
        private Command _currentServerProcess;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiAutomatorServer"/> class.
        /// </summary>
        /// <param name="terminal">Object to interact with the terminal.</param>
        /// <param name="port">The local port.</param>
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

        private string RpcUrl => $"{BaseUrl}/jsonrpc/0";

        private string StopUrl => $"{BaseUrl}/stop";

        /// <summary>
        /// Start the ui automator server on the android device.
        /// </summary>
        /// <exception cref="UiAutomatorServerException">The exception thrown if we can't server.</exception>
        public void Start()
        {
            DeviceLogger.Log("Starting server..");
            ForwardPorts();
            if (_currentServerProcess == null || _currentServerProcess.Process.HasExited)
            {
                DeviceLogger.Log("Starting instrumental");
                _currentServerProcess = _terminal.StartAdbProcess(
                    "shell",
                    "uiautomator",
                    "runtest",
                    DeviceConfiguration.UiAutomatorStubBundle,
                    DeviceConfiguration.UiAutomatorStub,
                    "-c",
                    "com.github.uiautomatorstub.Stub");
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
        /// Stop the ui automator server on the android device.
        /// </summary>
        public void Stop()
        {
            DeviceLogger.Log("Stopping server");
            using (var client = new HttpClient())
            {
                try
                {
                    client.GetAsync(StopUrl);
                }
                catch (Exception)
                {
                    // Because of the uiautomator stub this is normal response to a stop request
                }
            }

            if (_currentServerProcess != null)
            {
                KillProcessAndChildrens(_currentServerProcess.Process.Id);
            }
        }

        /// <summary>
        /// Check if the ui automator server is alive on the android device.
        /// </summary>
        /// <param name="timeout">Timeout in seconds.</param>
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

            DeviceLogger.Log("Server is not alive!");
            return false;
        }

        /// <summary>
        /// Send a screen dump request to the ui automator server on the android device.
        /// </summary>
        /// <returns>The screen content as a xml string.</returns>
        public string DumpUi()
        {
            var response = SendRpc(RpcUrl, "{\"jsonrpc\":2.0,\"method\":\"dumpWindowHierarchy\",\"id\":1,\"params\":[\"false\",null]}");
            if (string.IsNullOrEmpty(response))
            {
                DeviceLogger.Log("Failed to dump!");
                throw new UiAutomatorServerException("Could connect to server but the dumped ui was empty");
            }

            var dump = Regex.Match(response, "(<\\?xml)(.*)(</hierarchy>)");
            return dump.Value.Replace("\\\"", "\"");
        }

        /// <summary>
        /// Forward ports to the android device.
        /// </summary>
        private void ForwardPorts()
        {
            DeviceLogger.Log("Forwarding ports");
            _terminal.ExecuteAdbCommand("forward", $"tcp:{_localPort}", $"tcp:{DevicePort}");
        }

        /// <summary>
        /// Ping the ui automator server on the android device.
        /// </summary>
        /// <returns>True if we got response, false otherwise.</returns>
        private bool Ping()
        {
            try
            {
                var response = SendRpc(RpcUrl, "{\"jsonrpc\":2.0,\"method\":\"ping\",\"id\":1}");
                return response.Contains("pong");
            }
            catch (UiAutomatorServerException)
            {
                return false;
            }
        }

        /// <summary>
        /// Send a rpc to the server.
        /// </summary>
        /// <param name="url">The url</param>
        /// <param name="data">The rpc command in json format.</param>
        /// <returns>The response from the server, empty no response.</returns>
        private string SendRpc(string url, string data)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(Timeout);
                    var response = httpClient.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));
                    return response.Result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex) when (ex is AggregateException || ex is HttpRequestException || ex is WebException)
            {
                throw new UiAutomatorServerException("Could not connect with server", ex);
            }
        }

        private void KillProcessAndChildrens(int pid)
        {
            var processSearcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
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
