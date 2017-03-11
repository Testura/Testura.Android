using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util.Exceptions
{
    public class UiNodeNotFoundException : Exception
    {
        public UiNodeNotFoundException()
        {
        }

        public UiNodeNotFoundException(IList<With> withs)
            : base(WithErrorMessageBuilder.BuildWithErrorMessage(withs))
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
