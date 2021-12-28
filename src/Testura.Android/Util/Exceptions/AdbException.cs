namespace Testura.Android.Util.Exceptions
{
    /// <summary>
    /// Represent an exception thrown when having problem with ADB
    /// </summary>
    public class AdbException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdbException"/> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AdbException(string message)
            : base(message)
        {
        }
    }
}
