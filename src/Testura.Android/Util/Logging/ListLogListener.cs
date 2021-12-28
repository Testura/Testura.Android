namespace Testura.Android.Util.Logging
{
    /// <summary>
    /// Provides functionality to save all logs into a in-memory list.
    /// </summary>
    public class ListLogListener : ILogListener
    {
        private readonly DeviceLogger.LogLevel _minLogLevel;
        private readonly List<string> _logMessages;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLogListener"/> class.
        /// </summary>
        /// <param name="minLogLevel">Minimum log level to save</param>
        public ListLogListener(DeviceLogger.LogLevel minLogLevel)
        {
            _minLogLevel = minLogLevel;
            _logMessages = new List<string>();
        }

        /// <summary>
        /// Gets all saved log messages
        /// </summary>
        public IReadOnlyList<string> LogMessages => _logMessages;

        /// <summary>
        /// Write a new log message
        /// </summary>
        /// <param name="className">Name of the calling class</param>
        /// <param name="memberName">Method or property name of the calling class</param>
        /// <param name="message">Message from the calling class</param>
        /// <param name="logLevel">The message log level</param>
        public void Log(string className, string memberName, string message, DeviceLogger.LogLevel logLevel)
        {
            if (logLevel < _minLogLevel)
            {
                return;
            }

            _logMessages.Add($"[{logLevel.ToString()}][{DateTime.Now}][{className}][{memberName}]: {message}");
        }
    }
}
