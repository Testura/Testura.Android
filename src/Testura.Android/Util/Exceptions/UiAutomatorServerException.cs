#pragma warning disable 1591

namespace Testura.Android.Util.Exceptions
{
    /// <summary>
    /// Represent an exception thrown when we can't communicate with the UI automator server.
    /// </summary>
    public class UiAutomatorServerException : Exception
    {
        public UiAutomatorServerException(string message)
            : base(message)
        {
        }

        public UiAutomatorServerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
