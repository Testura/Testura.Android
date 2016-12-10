using System;

namespace Testura.Android.Util.Exceptions
{
    public class AdbException : Exception
    {
        public AdbException(string message)
            : base(message)
        {
        }
    }
}
