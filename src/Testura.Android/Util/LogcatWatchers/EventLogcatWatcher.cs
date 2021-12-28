using Testura.Android.Device;

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
        /// <param name="adbCommandExecutor">The adb terminal used to send adb commands</param>
        /// <param name="tags">A set of logcat tags.</param>
        /// <param name="flushLogcat">If we should flush logcat before starting.</param>
        public EventLogcatWatcher(AdbCommandExecutor adbCommandExecutor, IEnumerable<string> tags, bool flushLogcat = false)
            : base(adbCommandExecutor, tags, flushLogcat)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogcatWatcher"/> class.
        /// </summary>
        /// <param name="adbCommandExecutorProvider">An adb terminal provider</param>
        /// <param name="tags">A set of logcat tags.</param>
        /// <param name="flushLogcat">If we should flush logcat before starting.</param>
        public EventLogcatWatcher(IAdbCommandExecutorProvider adbCommandExecutorProvider, IEnumerable<string> tags, bool flushLogcat = false)
            : base(adbCommandExecutorProvider, tags, flushLogcat)
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
