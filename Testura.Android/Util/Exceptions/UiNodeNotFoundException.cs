using System;
using Testura.Android.UiAutomator;

namespace Testura.Android.Util.Exceptions
{
    public class UiNodeNotFoundException : Exception
    {
        public UiNodeNotFoundException(By by)
            : base("Didn't find node")
        {
            // do
        }
    }
}
