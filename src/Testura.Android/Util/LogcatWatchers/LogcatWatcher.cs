using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Testura.Android.Device.Configurations;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Util.LogcatWatchers
{
    public abstract class LogcatWatcher
    {
        private readonly ITerminal _terminal;
        private readonly IEnumerable<string> _tags;
        private readonly bool _flushLogcat;
        private CancellationTokenSource _cancellationTokenSource;

        protected LogcatWatcher(DeviceConfiguration deviceConfiguration, IEnumerable<string> tags, bool flushLogcat = false)
        {
            _terminal = new Terminal.Terminal(deviceConfiguration);
            _tags = tags;
            _flushLogcat = flushLogcat;
        }

        /// <summary>
        /// Start the logcat watcher task
        /// </summary>
        public void Start()
        {
            if (_flushLogcat)
            {
                _terminal.ExecuteAdbCommand(new[] { "logcat", "-c" });
            }

            _cancellationTokenSource = new CancellationTokenSource();

            var commands = new List<string>
            {
                "shell",
                "logcat",
                "-s"
            };

            commands.AddRange(_tags);
            var process = _terminal.StartAdbProcessWithoutShell(commands.ToArray());
            var reader = new StreamReader(process.StandardOutput.BaseStream);
            var cancellationToken = _cancellationTokenSource.Token;
            Task.Run(
                () =>
                    {
                        while (true)
                        {
                            var output = reader.ReadLine();
                            if (!string.IsNullOrEmpty(output))
                            {
                                NewOutput(output);
                            }
                            if (_cancellationTokenSource.IsCancellationRequested)
                            {
                                return;
                            }
                        }
                    },
                cancellationToken);
        }

        /// <summary>
        /// Stop the logcat watcher task
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Called when we receive another matching line from logcat
        /// </summary>
        /// <param name="output">Last matching line from logcat</param>
        protected abstract void NewOutput(string output);
    }
}
