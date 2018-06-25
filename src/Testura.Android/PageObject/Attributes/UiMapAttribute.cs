using System;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util;

namespace Testura.Android.PageObject.Attributes
{
    /// <summary>
    /// Provides functionality to automatically initialize an UIObject that use this attribute and exist on a <see cref="View"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class UiMapAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UiMapAttribute"/> class.
        /// </summary>
        /// <param name="with">Find node with this attribute tag.</param>
        /// <param name="value">Find node with this value on the wanted attribute tag.</param>
        public UiMapAttribute(AttributeTag with, string value)
        {
            switch (with)
            {
                case AttributeTag.TextContains:
                    With = With.ContainsText(value);
                    break;
                case AttributeTag.Text:
                    With = With.Text(value);
                    break;
                case AttributeTag.ResourceId:
                    With = With.ResourceId(value);
                    break;
                case AttributeTag.ContentDesc:
                    With = With.ContentDesc(value);
                    break;
                case AttributeTag.Class:
                    With = With.Class(value);
                    break;
                case AttributeTag.Package:
                    With = With.Package(value);
                    break;
                case AttributeTag.Index:
                    With = With.Index(int.Parse(value));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(with), with, null);
            }
        }

        /// <summary>
        /// Gets or sets the with that should be used to initialize the UI object.
        /// </summary>
        public With With { get; set; }
    }
}
