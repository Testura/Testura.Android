using System;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util;

namespace Testura.Android.PageObject.Attributes
{
    /// <summary>
    /// Initialize an UIObject automatically by using this attribute
    /// on a field/property inside a class that inherit from "View".
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CreateAttribute : Attribute
    {
        public CreateAttribute(AttributeTags with, string value)
        {
            switch (with)
            {
                case AttributeTags.TextContains:
                    With = With.ContainsText(value);
                    break;
                case AttributeTags.Text:
                    With = With.Text(value);
                    break;
                case AttributeTags.ResourceId:
                    With = With.ResourceId(value);
                    break;
                case AttributeTags.ContentDesc:
                    With = With.ContentDesc(value);
                    break;
                case AttributeTags.Class:
                    With = With.Class(value);
                    break;
                case AttributeTags.Package:
                    With = With.Package(value);
                    break;
                case AttributeTags.Index:
                    With = With.Index(int.Parse(value));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(with), with, null);
            }
        }

        public With With { get; set; }
    }
}
