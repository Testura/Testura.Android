using System;

namespace Testura.Android.Util.Exceptions
{
    public class UiAutomatorServerException : Exception
    {
        public UiAutomatorServerException(string message) 
            : base(message)
        {
        }
    }
}
