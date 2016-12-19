using System;

namespace Testura.Android.Util.Logging
{
    /// <summary>
    /// A simple console log listener that write log messages
    /// to the console.
    /// </summary>
    public class ConsoleLogListener : ILogListener
    {
        /// <summary>
        /// Write log message to the console
        /// </summary>
        /// <param name="className">Name of the calling class</param>
        /// <param name="memberName">Method or property name of the calling class</param>
        /// <param name="message">Message from the calling class</param>
        public void Log(string className, string memberName, string message)
        {
            Console.WriteLine($"[{DateTime.Now}][{className}][{memberName}]: {message}");
        }
    }
}
