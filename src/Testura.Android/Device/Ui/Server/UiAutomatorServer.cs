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
    /// <summary>
    /// Provides functionality to interact with the UI automator server
    /// </summary>
    public class UiAutomatorServer : IUiAutomatorServer
    {
        /// <summary>
        /// Get the server package name
        /// </summary>
        private const string AndroidPackageName = "com.testura.server";

        /// <summary>
        /// Get the device port.
        /// </summary>
        private const int DevicePort = 9020;

        /// <summary>
        /// Get the timeout in seconds.
        /// </summary>
        private const int Timeout = 5;

        private readonly int _localPort;
        private readonly object _serverLock;
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
            _serverLock = new object();
        }

        private string BaseUrl => $"http://localhost:{_localPort}";

        private string PingUrl => $"{BaseUrl}/ping";

        private string DumpUrl => $"{BaseUrl}/dump";

        private string StopUrl => $"{BaseUrl}/stop";

        /// <summary>
        /// Start the ui automator server on the android device.
        /// </summary>
        /// <exception cref="UiAutomatorServerException">The exception thrown if we can't server.</exception>
        public void Start()
        {
            lock (_serverLock)
            {
                DeviceLogger.Log("Starting server..");
                if (_currentServerProcess == null || _currentServerProcess.Process.HasExited)
                {
                    ForwardPorts();
                    KillAndroidProcess();
                    DeviceLogger.Log("Starting instrumental");
                    _currentServerProcess = _terminal.StartAdbProcess(
                        "shell",
                        $"am instrument -w -r -e debug false -e class {AndroidPackageName}.Start {AndroidPackageName}.test/android.support.test.runner.AndroidJUnitRunner");
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
        }

        /// <summary>
        /// Stop the ui automator server on the android device.
        /// </summary>
        public void Stop()
        {
            lock (_serverLock)
            {
                DeviceLogger.Log("Stopping server");
                try
                {
                    using (var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
                    {
                        client.GetAsync(StopUrl);
                    }
                }
                catch (Exception ex) when (ex is AggregateException || ex is HttpRequestException || ex is WebException)
                {
                    DeviceLogger.Log($"Failed stop server, already closed? {ex.Message}");
                }

                if (_currentServerProcess != null)
                {
                    KillLocalProcess(_currentServerProcess.Process.Id);
                }

                // Kill android process just to be safe..
                KillAndroidProcess();
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
            using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(3) })
            {
                try
                {
                    var repsonse = client.GetAsync(DumpUrl);
                    var dump = repsonse.Result.Content.ReadAsStringAsync().Result;
                    if (string.IsNullOrEmpty(dump))
                    {
                        DeviceLogger.Log("Failed to dump!");
                        throw new UiAutomatorServerException("Could connect to server but the dumped ui was empty");
                    }

                    return dump.Replace("\\\"", "\"");
                }
                catch (AggregateException ex)
                {
                    throw new UiAutomatorServerException("Faild to dump screen.", ex);
                }
            }
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
                using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
                {
                    var repsonse = client.GetAsync(PingUrl);
                    return repsonse.Result.Content.ReadAsStringAsync().Result == "Hello human.";
                }
            }
            catch (Exception ex) when (ex is AggregateException || ex is HttpRequestException || ex is WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Kill the local process and it's children.
        /// </summary>
        /// <param name="pid">The process id.</param>
        private void KillLocalProcess(int pid)
        {
            var processSearcher =
                new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
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
                    KillLocalProcess(Convert.ToInt32(mo["ProcessID"]));
                }
            }
        }

        /// <summary>
        /// Kill the android process.
        /// </summary>
        private void KillAndroidProcess()
        {
            try
            {
                _terminal.ExecuteAdbCommand("shell", $"ps | grep {AndroidPackageName}");
                DeviceLogger.Log("Killing testura helper process on the device.");
                _terminal.ExecuteAdbCommand("shell", $"pm clear {AndroidPackageName}");
            }
            catch (Exception)
            {
                // The terminal throw an exception if we can't find anything with grep.
            }
        }
    }
}