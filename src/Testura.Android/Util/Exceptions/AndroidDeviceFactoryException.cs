namespace Testura.Android.Util.Exceptions
{
    /// <summary>
    /// Represent an exception thrown when having problems in the android device factory
    /// </summary>
    public class AndroidDeviceFactoryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidDeviceFactoryException"/> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AndroidDeviceFactoryException(string message)
            : base(message)
        {
        }
    }
}
