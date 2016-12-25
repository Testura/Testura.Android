using System;

namespace Testura.Android.Util.Exceptions
{
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
