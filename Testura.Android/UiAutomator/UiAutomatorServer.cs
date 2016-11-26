using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        public async Task Start()
        {
            SetLocalPort();
            currentServerProcess = _terminal.StartTerminalProcess(
                "adb shell uiautomator runtest bundle.jar uiautomator-stub.jar -c com.github.uiautomatorstub.Stub");
            ForwardPorts();
            if (!await Alive())
            {
                throw new UiAutomatorServerException("Could not start server");
            }
        }

        public async Task<bool> Alive()
        {
            var time = DateTime.Now;
            while ((DateTime.Now - time).Seconds < 40)
            {
                var result = await Ping();
                if (result)
                {
                    return true;
                }
            }

            return false;
        }

        public string DumpUi()
        {
            return SendCommand("dumpWindowHierarchy", false, null).Result;
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

        private async Task<bool> Ping()
        {
            var result = await SendCommand("ping");
            return result == "pong";
        }

        private async Task<string> SendCommand(string methodName, params object[] args)
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
            var response = await httpClient.PostAsync($"http://localhost:{LocalPort}/jsonrpc/0", new StringContent(data, Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            dynamic parsedResult = JObject.Parse(result);
            return parsedResult.result;
        }
    }
}
