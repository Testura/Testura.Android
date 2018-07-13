using System;

namespace Testura.Android.Util.Exceptions
{
    public class MapUiObjectException : Exception
    {
        public MapUiObjectException(string message) : base(message)
        {
        }

        public MapUiObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
