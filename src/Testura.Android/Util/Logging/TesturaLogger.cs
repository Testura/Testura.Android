using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Testura.Android.Util.Logging
{
    internal static class TesturaLogger
    {
        private static IList<ILogListener> _logListeners;

        /// <summary>
        /// Add a new log listener to our log listener list
        /// </summary>
        /// <param name="logListener">Log listener to add</param>
        internal static void AddListener(ILogListener logListener)
        {
            if (_logListeners == null)
            {
                _logListeners = new List<ILogListener>();
            }

            _logListeners.Add(logListener);
        }


        /// <summary>
        /// Write a new log message to all log listeners
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="memberName">Method or property name of the caller</param>
        /// <param name="sourceFilePath">Full path of the source file that contains the caller.</param>
        internal static void Log(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "")
        {
            if (_logListeners == null)
            {
                return;
            }

            foreach (var logListener in _logListeners)
            {
                logListener.Log(Path.GetFileNameWithoutExtension(sourceFilePath), memberName, message);
            }
        }
    }
}
