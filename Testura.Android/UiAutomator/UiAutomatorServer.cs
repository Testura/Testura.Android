using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Testura.Android.Util;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.UiAutomator
{
    public class UiAutomatorServer : IUiAutomatorServer
    {
        public const int DefaultLocalPort = 9009;
        public const int DevicePort = 9008;

        private readonly ITerminal _terminal;
        private Process currentServerProcess;

        public UiAutomatorServer(ITerminal terminal)
        {
            _terminal = terminal;
        }

        public int LocalPort { get; private set; }

        public void Start()
        {
            SetLocalPort();
            currentServerProcess =
                _terminal.StartTerminalProcess(
                    "adb shell am instrument -w -r -e debug false -e class com.testura.testuraandroidserver.Start#RunServer com.testura.testuraandroidserver.test/android.support.test.runner.AndroidJUnitRunner");
            ForwardPorts();
            if (!Alive(10))
            {
                throw new UiAutomatorServerException("Could not start server");
            }
        }

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

        public string DumpUi()
        {
            return SendCommand("dumpWindowHierarchy");
        }

        private void SetLocalPort()
        {
            var output = _terminal.ExecuteCommand("adb forward --list");
            var lines = output.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (!lines.Any())
            {
                LocalPort = DefaultLocalPort;
                return;
            }

            LocalPort =
                Int32.Parse(
                    lines.First().Split(' ')[1].Split(new[] { "tcp:" }, StringSplitOptions.RemoveEmptyEntries).First());
        }

        private void ForwardPorts()
        {
            _terminal.ExecuteCommand($"adb forward tcp:{LocalPort} tcp:{DevicePort}");
        }

        private bool Ping()
        {
            try
            {
                var result = SendCommand("ping");
                return result == "hello";
            }
            catch (Exception ex) when (ex is AggregateException || ex is HttpRequestException || ex is WebException)
            {
                return false;
            }
        }

        private string SendCommand(string methodName, params object[] args)
        {
            var data = JsonConvert.SerializeObject(
                new
                {
                    jsonrpc = "2.0",
                    method = methodName,
                    id = 1,
                    @params = args
                });
            var httpClient = new HttpClient();
            var response =
                httpClient.PostAsync($"http://localhost:{LocalPort}/jsonrpc/0",
                    new StringContent(data, Encoding.UTF8, "application/json")).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            dynamic parsedResult = JObject.Parse(result);
            return parsedResult.result;
        }
    }
}
