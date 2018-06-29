using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util;

namespace Testura.Android.PageObject.Attributes
{
    /// <summary>
    /// Provides functionality to automatically initialize an UIObject that use this attribute and exist on a <see cref="View"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MapUiObjectAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapUiObjectAttribute"/> class.
        /// </summary>
        public MapUiObjectAttribute(
            string text = null,
            string containsText = null,
            string resourceId = null,
            string contentDesc = null,
            string @class = null,
            string package = null,
            string index = null)
        {
            Bys = new List<By>();

            if (text != null)
            {
                Bys.Add(By.Text(text));
            }

            if (containsText != null)
            {
                Bys.Add(By.ContainsText(containsText));
            }

            if (resourceId != null)
            {
                Bys.Add(By.ResourceId(resourceId));
            }

            if (contentDesc != null)
            {
                Bys.Add(By.ContentDesc(contentDesc));
            }

            if (@class != null)
            {
                Bys.Add(By.Class(@class));
            }

            if (package != null)
            {
                Bys.Add(By.Package(package));
            }

            if (index != null)
            {
                Bys.Add(By.Index(int.Parse(index)));
            }

            if (!Bys.Any())
            {
                throw new ArgumentException("Need to provide at least one \"by\" when mapping ui object", nameof(By));
            }
        }

        /// <summary>
        /// Gets or sets the bys that should be used to map the ui object.
        /// </summary>
        public IList<By> Bys { get; set; }
    }
}
