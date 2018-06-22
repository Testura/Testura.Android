using System;
using Testura.Android.Device;
using Testura.Android.Device.Configurations;

namespace Testura.Android.Util.LogcatWatchers
{
    /// <summary>
    /// Provides a log watcher that send out a new event every time we get a new matching log message.
    /// </summary>
    public class EventLogcatWatcher : LogcatWatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogcatWatcher"/> class.
        /// </summary>
        /// <param name="deviceConfiguration">Current device configuration.</param>
        /// <param name="tags">A set of logcat tags.</param>
        /// <param name="flushLogcat">If we should flush logcat before starting.</param>
        public EventLogcatWatcher(IAdbTerminalContainer adbTerminalHandler, string[] tags, bool flushLogcat = false)
            : base(adbTerminalHandler, tags, flushLogcat)
        {
        }

        /// <summary>
        /// NewOutputEvent is raised every time we receive another matching line from logcat
        /// </summary>
        public event EventHandler<string> NewOutputEvent;

        /// <summary>
        /// Called when we receive another matching line from logcat
        /// </summary>
        /// <param name="output">Last matching line from logcat</param>
        protected override void NewOutput(string output)
        {
            NewOutputEvent?.Invoke(this, output);
        }
    }
}
