using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using Medallion.Shell;
using Testura.Android.Util;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Http;
using Testura.Android.Util.Logging;

namespace Testura.Android.Device.Ui.Server
{
    /// <summary>
    /// Provides functionality to interact with the UI automator server
    /// </summary>
    public class UiAutomatorServer : IUiAutomatorServer, IInteractionUiAutomatorServer
    {
        /// <summary>
        /// Get the server package name
        /// </summary>
        private const string AndroidPackageName = "com.testura.server";

        /// <summary>
        /// Get the device port.
        /// </summary>
        private const int DevicePort = 9020;

        private readonly int _localPort;
        private readonly int _dumpTimeout;
        private readonly object _serverLock;
        private readonly Terminal _terminal;
        private readonly HttpClient _httpClient;
        private Command _currentServerProcess;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiAutomatorServer"/> class.
        /// </summary>
        /// <param name="terminal">Object to interact with the terminal.</param>
        /// <param name="port">The local port.</param>
        /// <param name="dumpTimeout">Dump timeout in sec</param>
        public UiAutomatorServer(Terminal terminal, int port, int dumpTimeout)
        {
            _localPort = port;
            _dumpTimeout = dumpTimeout;
            _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));
            _serverLock = new object();
            _httpClient = new HttpClient(new TimeoutHandler(TimeSpan.FromSeconds(10), new HttpClientHandler()));
        }

        private string BaseUrl => $"http://localhost:{_localPort}";

        private string PingUrl => $"{BaseUrl}/ping";

        private string DumpUrl => $"{BaseUrl}/dump";

        private string TapUrl => $"{BaseUrl}/tap";

        private string SwipeUrl => $"{BaseUrl}/swipe";

        private string InputTextUrl => $"{BaseUrl}/inputText";

        private string InputKeyEventUrl => $"{BaseUrl}/inputKeyEvent";

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

                ForwardPorts();
                KillAndroidProcess();
                DeviceLogger.Log("Starting instrumental");
                _currentServerProcess = _terminal.StartAdbProcessWithoutShell(
                    "shell",
                    $"am instrument -w -r -e debug false -e class {AndroidPackageName}.Start {AndroidPackageName}.test/android.support.test.runner.AndroidJUnitRunner");

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
                    var request = new HttpRequestMessage(HttpMethod.Get, StopUrl);
                    request.SetTimeout(TimeSpan.FromSeconds(5));
                    _httpClient.SendAsync(request);
                }
                catch (Exception ex) when (ex is AggregateException || ex is HttpRequestException || ex is WebException)
                {
                    DeviceLogger.Log($"Failed stop server, already closed? {ex.Message}");
                }

               _currentServerProcess.Kill();
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
            while ((DateTime.Now - time).TotalSeconds < timeout)
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
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, DumpUrl);
                request.SetTimeout(TimeSpan.FromSeconds(_dumpTimeout));
                var repsonse = _httpClient.SendAsync(request).Result;
                var dump = repsonse.Content.ReadAsStringAsync().Result;

                if (string.IsNullOrEmpty(dump))
                {
                    DeviceLogger.Log("Empty dump from server!");
                    DeviceLogger.Log($"Status code: {repsonse.StatusCode}");
                    DeviceLogger.Log($"Reason phrase: {repsonse.ReasonPhrase}");
                    throw new UiAutomatorServerException("Could connect to server but the dumped ui was empty");
                }

                return dump.Replace("\\\"", "\"");
            }
            catch (Exception ex)
            {
                DeviceLogger.Log("Unexpected error/timed out when trying to dump: ");
                DeviceLogger.Log(ex.ToString());
                throw new UiAutomatorServerException("Failed to dump screen", ex);
            }
        }

        /// <summary>
        /// Send a tap request to the ui automator server on the android device.
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>True if we successfully tapped, otherwise false.</returns>
        public bool Tap(int x, int y)
        {
            return SendInteractionRequest($"{TapUrl}?x={x}&y={y}", TimeSpan.FromMilliseconds(3000));
        }

        /// <summary>
        /// Send a swipe request to the ui automator server on the android device.
        /// </summary>
        /// <param name="fromX">Swipe from this x coordinate</param>
        /// <param name="fromY">Swipe from this y coordinate</param>
        /// <param name="toX">Swipe to this x coordinate</param>
        /// <param name="toY">Swipe to this y coordinate</param>
        /// <param name="duration">Swipe duration in miliseconds</param>
        /// <returns>True if we successfully swiped, otherwise false.</returns>
        public bool Swipe(int fromX, int fromY, int toX, int toY, int duration)
        {
            return SendInteractionRequest(
                $"{SwipeUrl}?startX={fromX}&startY={fromY}&endX={toX}&endY={toY}&step={duration / 25}",
                TimeSpan.FromMilliseconds(3000 + duration));
        }

        /// <summary>
        /// Send a key event request to the ui automator server on the android device.
        /// </summary>
        /// <param name="keyEvent">Key event to send to the device</param>
        /// <returns>True if we successfully input key event, otherwise false.</returns>
        public bool InputKeyEvent(KeyEvents keyEvent)
        {
            return SendInteractionRequest($"{InputKeyEventUrl}?keyEvent={(int) keyEvent}",
                TimeSpan.FromMilliseconds(3000));
        }

        /// <summary>
        /// Send a input text request to the ui automator server on the android device.
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <returns>True if we successfully input text, otherwise false.</returns>
        public bool InputText(string text)
        {
            text = HttpUtility.UrlEncode(text);
            return SendInteractionRequest($"{InputTextUrl}?text={text}", TimeSpan.FromMilliseconds(3000));
        }

        /// <summary>
        /// Send interaction request to server
        /// </summary>
        /// <param name="url">Url of the request</param>
        /// <param name="timeout">Timeout of request</param>
        /// <returns>True if we managed to perform interaction, otherwise false.</returns>
        private bool SendInteractionRequest(string url, TimeSpan timeout)
        {
            if (!Alive(5))
            {
                Start();
            }

            DeviceLogger.Log($"Sending interaction request to server: {url}");

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.SetTimeout(timeout);
                var response = _httpClient.SendAsync(request).Result;
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new UiAutomatorServerException(
                            "Server responded with 404, make sure that you have the latest Testura server app.");
                    }

                    return false;
                }

                var result = response.Content.ReadAsStringAsync().Result;
                return result == "success";
            }
            catch (Exception ex)
            {
                DeviceLogger.Log("interaction request timed out");
                DeviceLogger.Log(ex.Message);
                return false;
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
                // Use a token just to make sure we don't get stuck..
                using (var cts = new CancellationTokenSource())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, PingUrl);

                    cts.CancelAfter(TimeSpan.FromSeconds(15));
                    request.SetTimeout(TimeSpan.FromSeconds(10));

                    return _httpClient.SendAsync(request, cts.Token).Result.Content.ReadAsStringAsync().Result == "Hello human.";
                }
            }
            catch (Exception ex) when (ex is AggregateException || ex is HttpRequestException || ex is WebException)
            {
                return false;
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