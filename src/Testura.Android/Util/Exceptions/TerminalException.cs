using System;

namespace Testura.Android.Util.Exceptions
{
    public class TerminalException : Exception
    {
        public TerminalException(string message)
            : base(message)
        {
        }
    }
}
