using System;
using Testura.Android.Device.UiAutomator.Ui.Search;

namespace Testura.Android.Util.Exceptions
{
    public class UiNodeNotFoundException : Exception
    {
        public UiNodeNotFoundException(With[] with)
            : base("Could not find node.")
        {
        }

        public UiNodeNotFoundException(string message)
            : base(message)
        {
        }
    }
}
