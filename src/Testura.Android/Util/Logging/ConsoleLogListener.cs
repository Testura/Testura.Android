namespace Testura.Android.Util.Logging
{
    /// <summary>
    /// A simple console log listener that write log messages
    /// to the console.
    /// </summary>
    public class ConsoleLogListener : ILogListener
    {
        private readonly DeviceLogger.LogLevel _minLogLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogListener"/> class.
        /// </summary>
        /// <param name="minLogLevel">Minimum log level</param>
        public ConsoleLogListener(DeviceLogger.LogLevel minLogLevel)
        {
            _minLogLevel = minLogLevel;
        }

        /// <summary>
        /// Write log message to the console
        /// </summary>
        /// <param name="className">Name of the calling class</param>
        /// <param name="memberName">Method or property name of the calling class</param>
        /// <param name="message">Message from the calling class</param>
        /// <param name="logLevel">The message log level</param>´s
        public void Log(string className, string memberName, string message, DeviceLogger.LogLevel logLevel)
        {
            if (logLevel < _minLogLevel)
            {
                return;
            }

            Console.WriteLine($"[{logLevel.ToString()}][{DateTime.Now}][{className}][{memberName}]: {message}");
        }
    }
}
