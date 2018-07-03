using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.PageObject.Attributes
{
    /// <summary>
    /// Provides functionality to automatically initialize an UIObject that use this attribute and exist on a <see cref="View"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MapUiObjectAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the text used to map the <see cref="UiObject">ui object</see>
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the "text contains" used to map the <see cref="UiObject">ui object</see>
        /// </summary>
        public string TextContains { get; set; }

        /// <summary>
        /// Gets or sets the resource id used to map the <see cref="UiObject">ui object</see>
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the content desc used to map the <see cref="UiObject">ui object</see>
        /// </summary>
        public string ContentDesc { get; set; }

        /// <summary>
        /// Gets or sets the class used to map the <see cref="UiObject">ui object</see>
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the package used to map the <see cref="UiObject">ui object</see>
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// Gets or sets the index used to map the <see cref="UiObject">ui object</see>
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// Get all wheres that should be used to map this <see cref="UiObject">ui object</see>
        /// </summary>
        /// <returns>Where(s) used to map the <see cref="UiObject">ui object</see></returns>
        public IList<Where> GetWheres()
        {
            var wheres = new List<Where>();

            if (Text != null)
            {
                wheres.Add(Where.Text(Text));
            }

            if (TextContains != null)
            {
                wheres.Add(Where.TextContains(TextContains));
            }

            if (ResourceId != null)
            {
                wheres.Add(Where.ResourceId(ResourceId));
            }

            if (ContentDesc != null)
            {
                wheres.Add(Where.ContentDesc(ContentDesc));
            }

            if (Class != null)
            {
                wheres.Add(Where.Class(Class));
            }

            if (Package != null)
            {
                wheres.Add(Where.Package(Package));
            }

            if (Index != null)
            {
                wheres.Add(Where.Index(int.Parse(Index)));
            }

            if (!wheres.Any())
            {
                throw new ArgumentException("Need to provide at least one \"with\" when mapping ui object", nameof(Where));
            }

            return wheres;
        }
    }
}
