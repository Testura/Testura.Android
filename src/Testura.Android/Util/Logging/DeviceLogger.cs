using System.Runtime.CompilerServices;

namespace Testura.Android.Util.Logging
{
    /// <summary>
    /// Provides functionality to log and listen to log.
    /// </summary>
    public static class DeviceLogger
    {
        private static IList<ILogListener> _logListeners;

        /// <summary>
        /// Defines logging severity levels.
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// Logs that are used for interactive investigation during development. These logs should primarily contain information useful for debugging and have no long-term value.
            /// </summary>
            Debug,

            /// <summary>
            /// Logs that track the general flow of the application. These logs should have long-term value.
            /// </summary>
            Info,

            /// <summary>
            /// Logs that highlight an abnormal or unexpected event in the application flow, but do not otherwise cause the application execution to stop.
            /// </summary>
            Warning,

            /// <summary>
            /// Logs that describe an unrecoverable application or system crash, or a catastrophic failure that requires immediate attention.
            /// </summary>
            Error
        }

        /// <summary>
        /// Add a new log listener to our log listener list
        /// </summary>
        /// <param name="logListener">Log listener to add</param>
        public static void AddListener(ILogListener logListener)
        {
            if (_logListeners == null)
            {
                _logListeners = new List<ILogListener>();
            }

            _logListeners.Add(logListener);
        }

        /// <summary>
        /// Get all current log listeners
        /// </summary>
        /// <returns>A list with all log listeners</returns>
        public static IReadOnlyList<ILogListener> GetListeners()
        {
            if (_logListeners == null)
            {
                return new List<ILogListener>();
            }

            return new List<ILogListener>(_logListeners);
        }

        /// <summary>
        /// Remove all log listeners
        /// </summary>
        /// <param name="logListener">Log listener to remove</param>
        public static void RemoveListener(ILogListener logListener)
        {
            if (_logListeners == null)
            {
                return;
            }

            if (_logListeners.Contains(logListener))
            {
                _logListeners.Remove(logListener);
            }
        }

        /// <summary>
        /// Write a new log message to all log listeners
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="logLevel">Log level</param>
        /// <param name="memberName">Method or property name of the caller</param>
        /// <param name="sourceFilePath">Full path of the source file that contains the caller.</param>
        internal static void Log(
            string message,
            LogLevel logLevel,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "")
        {
            if (_logListeners == null)
            {
                return;
            }

            foreach (var logListener in _logListeners)
            {
                logListener.Log(Path.GetFileNameWithoutExtension(sourceFilePath), memberName, message, logLevel);
            }
        }
    }
}
