using System;
using System.Collections.Generic;
using Testura.Android.UiAutomator.Search;

namespace Testura.Android.Util.Exceptions
{
    public class UiNodeNotFoundException : Exception
    {
        public UiNodeNotFoundException(IList<NodeSearch> nodeSearch)
            : base("Didn't find node")
        {
            // do
        }
    }
}
