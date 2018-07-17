using System;
using System.Collections.Generic;
using Testura.Android.Device.Ui.Search;
#pragma warning disable 1591

namespace Testura.Android.Util.Exceptions
{
    /// <summary>
    /// Represent an exception thrown when we can't find an UI Node.
    /// </summary>
    public class UiNodeNotFoundException : Exception
    {
        public UiNodeNotFoundException()
        {
        }

        public UiNodeNotFoundException(IList<Where> wheres)
            : base(WithErrorMessageBuilder.BuildByErrorMessage(wheres))
        {
        }

        public UiNodeNotFoundException(string message)
            : base(message)
        {
        }

        public UiNodeNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
