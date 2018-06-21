namespace Testura.Android.Util.Logging
{
    /// <summary>
    /// Defines methods to work with the log listener.
    /// </summary>
    public interface ILogListener
    {
        /// <summary>
        /// Write a new log message
        /// </summary>
        /// <param name="className">Name of the calling class</param>
        /// <param name="memberName">Method or property name of the calling class</param>
        /// <param name="message">Message from the calling class</param>
        /// <param name="logLevel">The message log level</param>
        void Log(string className, string memberName, string message, DeviceLogger.LogLevels logLevel);
    }
}
