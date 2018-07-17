using System;

namespace Testura.Android.Util.Exceptions
{
    /// <summary>
    /// Represent an exception thrown when we fail to map ui object
    /// </summary>
    public class MapUiObjectException : Exception
    {
        /// <inheritdoc />
        public MapUiObjectException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        public MapUiObjectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
