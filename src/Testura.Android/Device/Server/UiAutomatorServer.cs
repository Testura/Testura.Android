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

namespace Testura.Android.Device.Server
{
    /// <summary>
    /// Provides functionality to interact with the UI automator server
    /// </summary>
    public class UiAutomatorServer : IUiAutomatorServer, IInteractionUiAutomatorServer
    {
        /// <summary>
        /// Number of tries to send request to the server
        /// </summary>
        private const int RequestTries = 10;

        /// <summary>
        /// Get the server package name
        /// </summary>
        private const string AndroidPackageName = "com.testura.server";

        /// <summary>
        /// Get the device port.
        /// </summary>
        private const int DevicePort = 9020;

        private readonly object _serverLock;
        private readonly object _dumpLock;

        private readonly int _localPort;
        private readonly AdbCommandExecutor _adbCommandExecutor;
        private readonly HttpClient _httpClient;
        private Command _currentServerProcess;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiAutomatorServer"/> class.
        /// </summary>
        /// <param name="adbCommandExecutor">Object to interact with the terminal.</param>
        /// <param name="port">The local port.</param>
        public UiAutomatorServer(AdbCommandExecutor adbCommandExecutor, int port)
        {
            _localPort = port;
            _adbCommandExecutor = adbCommandExecutor ?? throw new ArgumentNullException(nameof(adbCommandExecutor));
            _serverLock = new object();
            _dumpLock = new object();
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
                DeviceLogger.Log("Starting server..", DeviceLogger.LogLevel.Info);

                ForwardPorts();
                KillAndroidProcess();
                DeviceLogger.Log("Starting instrumental", DeviceLogger.LogLevel.Info);
                _currentServerProcess = _adbCommandExecutor.StartAdbProcessWithoutShell(
                    "shell",
                    $"am instrument -w -r -e debug false -e class {AndroidPackageName}.Start {AndroidPackageName}.test/android.support.test.runner.AndroidJUnitRunner");

                if (!Alive(TimeSpan.FromSeconds(5)))
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
                DeviceLogger.Log("Stopping server", DeviceLogger.LogLevel.Info);
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, StopUrl);
                    request.SetTimeout(TimeSpan.FromSeconds(5));
                    _httpClient.SendAsync(request);
                }
                catch (Exception ex) when (ex is AggregateException || ex is HttpRequestException || ex is WebException)
                {
                    DeviceLogger.Log($"Failed stop server, already closed? {ex.Message}", DeviceLogger.LogLevel.Info);
                }

                _currentServerProcess?.Kill();
                KillAndroidProcess();
            }
        }

        /// <summary>
        /// Check if the ui automator server is alive on the android device.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>True if server is a alive, false otherwise.</returns>
        public bool Alive(TimeSpan timeout)
        {
            var time = DateTime.Now;
            DeviceLogger.Log("Checking if server is alive..", DeviceLogger.LogLevel.Debug);
            while ((DateTime.Now - time).TotalSeconds < timeout.TotalSeconds)
            {
                var result = Ping();
                if (result)
                {
                    return true;
                }
            }

            DeviceLogger.Log("Server is not alive!", DeviceLogger.LogLevel.Error);
            return false;
        }

        /// <summary>
        /// Send a screen dump request to the ui automator server on the android device.
        /// </summary>
        /// <returns>The screen content as a xml string.</returns>
        public string DumpUi()
        {
            lock (_dumpLock)
            {
                var tries = 0;
                while (tries < RequestTries)
                {
                    DeviceLogger.Log("Sending screen dump request", DeviceLogger.LogLevel.Debug);
                    var response = SendServerRequest(DumpUrl, TimeSpan.FromSeconds(15));
                    if (!string.IsNullOrEmpty(response))
                    {
                        return response.Replace("\\\"", "\"");
                    }

                    DeviceLogger.Log("Empty dump from server!", DeviceLogger.LogLevel.Info);

                    /* In some cases we get stuck and the server is alive
                       but we can't dump the UI. So try a reboot */
                    if (Alive(TimeSpan.FromSeconds(2)))
                    {
                        Stop();
                    }

                    tries--;
                    DeviceLogger.Log(
                        $"Screen dump request failed, trying {tries} more times",
                        DeviceLogger.LogLevel.Debug);
                }

                throw new UiAutomatorServerException("Failed to dump screen");
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
            DeviceLogger.Log($"Sending tap request ( x: {x}, y: {y}", DeviceLogger.LogLevel.Info);
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
            DeviceLogger.Log($"Sending swipe request ( fromX: {fromX}, fromY: {fromY}, toX: {toX}, toY: {toY}, duration: {duration})", DeviceLogger.LogLevel.Info);
            return SendInteractionRequest(
                $"{SwipeUrl}?startX={fromX}&startY={fromY}&endX={toX}&endY={toY}&step={duration / 25}",
                TimeSpan.FromMilliseconds(3000 + duration));
        }

        /// <summary>
        /// Send a key event request to the ui automator server on the android device.
        /// </summary>
        /// <param name="keyEvent">Key event to send to the device</param>
        /// <returns>True if we successfully input key event, otherwise false.</returns>
        public bool InputKeyEvent(KeyEvent keyEvent)
        {
            DeviceLogger.Log($"Sending keyevent request ({keyEvent.ToString()})", DeviceLogger.LogLevel.Info);
            return SendInteractionRequest($"{InputKeyEventUrl}?keyEvent={(int)keyEvent}", TimeSpan.FromMilliseconds(3000));
        }

        /// <summary>
        /// Send a input text request to the ui automator server on the android device.
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <returns>True if we successfully input text, otherwise false.</returns>
        public bool InputText(string text)
        {
            DeviceLogger.Log($"Sending text request (\"{text}\")", DeviceLogger.LogLevel.Info);
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
            var tries = 0;
            while (tries < RequestTries)
            {
                DeviceLogger.Log("Sending interaction request", DeviceLogger.LogLevel.Debug);
                var response = SendServerRequest(url, timeout);
                if (response == "success")
                {
                    DeviceLogger.Log("Interaction request was successful", DeviceLogger.LogLevel.Debug);
                    return true;
                }

                tries--;
                DeviceLogger.Log($"Interaction request failed, trying {tries} more times", DeviceLogger.LogLevel.Debug);
            }

            DeviceLogger.Log("Failed to send interaction request", DeviceLogger.LogLevel.Error);
            return false;
        }

        /// <summary>
        /// Send interaction request to server
        /// </summary>
        /// <param name="url">Url of the request</param>
        /// <param name="timeout">Timeout of request</param>
        /// <returns>True if we managed to perform interaction, otherwise false.</returns>
        private string SendServerRequest(string url, TimeSpan timeout)
        {
            if (!Alive(TimeSpan.FromSeconds(5)))
            {
                Start();
            }

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

                    DeviceLogger.Log("Server request was unsuccessful", DeviceLogger.LogLevel.Debug);
                    LogRespone(response);
                }

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                DeviceLogger.Log(ex.ToString(), DeviceLogger.LogLevel.Debug);
                return null;
            }
        }

        /// <summary>
        /// Forward ports to the android device.
        /// </summary>
        private void ForwardPorts()
        {
            DeviceLogger.Log("Forwarding ports", DeviceLogger.LogLevel.Info);
            _adbCommandExecutor.ExecuteAdbCommand("forward", $"tcp:{_localPort}", $"tcp:{DevicePort}");
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

                    var response = _httpClient.SendAsync(request, cts.Token).Result;

                    if (response.Content.ReadAsStringAsync().Result ==
                        "Hello human.")
                    {
                        return true;
                    }

                    DeviceLogger.Log("Ping server failed!", DeviceLogger.LogLevel.Debug);
                    LogRespone(response);
                    return false;
                }
            }
            catch (Exception ex) when (ex is AggregateException || ex is HttpRequestException || ex is WebException)
            {
                DeviceLogger.Log("ing server failed!", DeviceLogger.LogLevel.Debug);
                DeviceLogger.Log(ex.ToString(), DeviceLogger.LogLevel.Debug);
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
                _adbCommandExecutor.ExecuteAdbCommand("shell", $"ps | grep {AndroidPackageName}");
                DeviceLogger.Log("Killing testura helper process on the device.", DeviceLogger.LogLevel.Info);
                _adbCommandExecutor.ExecuteAdbCommand("shell", $"pm clear {AndroidPackageName}");
            }
            catch (Exception)
            {
                // The terminal throw an exception if we can't find anything with grep.
            }
        }

        private void LogRespone(HttpResponseMessage response)
        {
            DeviceLogger.Log("Response from server: ", DeviceLogger.LogLevel.Debug);
            DeviceLogger.Log($"HTTP status code: {response.StatusCode}", DeviceLogger.LogLevel.Debug);
            DeviceLogger.Log($"Reason phrase: {response.ReasonPhrase}", DeviceLogger.LogLevel.Debug);
            DeviceLogger.Log($"Content: {response.Content.ReadAsStringAsync().Result}", DeviceLogger.LogLevel.Debug);
        }
    }
}