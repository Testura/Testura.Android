using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Testura.Android.Util.Logging
{
    /// <summary>
    /// Provides functionality to log and listen to log.
    /// </summary>
    public static class DeviceLogger
    {
        public enum LogLevels
        {
            Debug,
            Info,
            Warning,
            Error,
        }

        private static IList<ILogListener> _logListeners;

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
            LogLevels logLevel,
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
