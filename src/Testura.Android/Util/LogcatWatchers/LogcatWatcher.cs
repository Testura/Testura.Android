using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Testura.Android.Device.Configurations;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util.LogcatWatchers
{
    /// <summary>
    /// Provides the base class for logcat watchers.
    /// </summary>
    public abstract class LogcatWatcher
    {
        private readonly Terminal _terminal;
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
        protected LogcatWatcher(Terminal terminal, IEnumerable<string> tags, bool flushLogcat = false)
        {
            _terminal = terminal;
            _tags = tags;
            _flushLogcat = flushLogcat;
        }

        /// <summary>
        /// Start the logcat watcher task.
        /// </summary>
        public void Start()
        {
            if (_task != null)
            {
                DeviceLogger.Log("Logcat watcher aldready started.. closing last process");
                Stop();
            }

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
                                process.Kill();
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
            _task = null;
        }

        /// <summary>
        /// Called when we receive another matching line from logcat.
        /// </summary>
        /// <param name="output">Last matching line from logcat.</param>
        protected abstract void NewOutput(string output);
    }
}
