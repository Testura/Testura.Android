using System;
#pragma warning disable 1591

namespace Testura.Android.Util.Exceptions
{
    /// <summary>
    /// Represent an exception thrown when we get unexpected problems with the terminal.
    /// </summary>
    public class TerminalException : Exception
    {
        public TerminalException(string message)
            : base(message)
        {
        }
    }
}
