using System;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.PageObject
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InitializeUiObjectAttribute : Attribute
    {
        public InitializeUiObjectAttribute(Action<int> h)
        {
            With = null;
        }

        public With[] With { get; set; }
    }
}
