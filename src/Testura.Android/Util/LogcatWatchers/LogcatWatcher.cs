using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Testura.Android.Device.Configurations;
using Testura.Android.Util.Logging;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Util.LogcatWatchers
{
    /// <summary>
    /// Provides the base class for logcat watchers.
    /// </summary>
    public abstract class LogcatWatcher
    {
        private readonly ITerminal _terminal;
        private readonly IEnumerable<string> _tags;
        private readonly bool _flushLogcat;
        private Task _task;
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogcatWatcher"/> class.
        /// </summary>
        /// <param name="deviceConfiguration">Current device configuration.</param>
        /// <param name="tags">A set of logcat tags.</param>
        /// <param name="flushLogcat">If we should flush logcat before starting.</param>
        protected LogcatWatcher(DeviceConfiguration deviceConfiguration, IEnumerable<string> tags, bool flushLogcat = false)
        {
            _terminal = new Terminal.Terminal(deviceConfiguration);
            _tags = tags;
            _flushLogcat = flushLogcat;
        }

        /// <summary>
        /// Start the logcat watcher task.
        /// </summary>
        public void Start()
        {
            DeviceLogger.Log("Starting logcat watcher..");

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
            var cancellationToken = _cancellationTokenSource.Token;
            _task = Task.Run(
                () =>
                {
                    process.StandardOutput.BaseStream.ReadTimeout = 500;

                        while (true)
                        {
                            try
                            {
                                var output = process.StandardOutput.ReadLine();
                                if (!string.IsNullOrEmpty(output))
                                {
                                    NewOutput(output);
                                }
                            }
                            catch (TimeoutException)
                            {
                            }

                            if (_cancellationTokenSource.IsCancellationRequested)
                            {
                                DeviceLogger.Log("Logcat watcher cancellation requested, stopping task.");
                                return;
                            }
                        }
                    },
                cancellationToken);
        }

        /// <summary>
        /// Stop the logcat watcher task.
        /// </summary>
        public void Stop()
        {
            DeviceLogger.Log("Request to stop logcat watcher..");
            _cancellationTokenSource?.Cancel();
            _task?.Wait(2000);
        }

        /// <summary>
        /// Called when we receive another matching line from logcat.
        /// </summary>
        /// <param name="output">Last matching line from logcat.</param>
        protected abstract void NewOutput(string output);
    }
}
