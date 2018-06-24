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
        public UiMapAttribute(AttributeTags with, string value)
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

        /// <summary>
        /// Gets or sets the with that should be used to initialize the UI object.
        /// </summary>
        public With With { get; set; }
    }
}
