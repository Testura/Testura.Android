using System;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Util;

namespace Testura.Android.Device.Ui.Search
{
    public class With
    {
        private With(Func<Node, bool> nodeSearch, string errorMessage)
        {
            NodeSearch = nodeSearch;
            ErrorMessage = errorMessage;
        }

        public Func<Node, bool> NodeSearch { get; }

        public string ErrorMessage { get; }

        /// <summary>
        /// Find node with matching text
        /// </summary>
        /// <param name="text">The text of the node to be found</param>
        /// <returns>An instance of the with object containing the search function</returns>
        public static With Text(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Argument is null or empty", nameof(text));
            }

            return Attribute(AttributeTags.Text, text);
        }

        /// <summary>
        /// Find node with text that contains text
        /// </summary>
        /// <param name="text">The partial text of the node to be found</param>
        /// <returns>An instance of the with object containing the search function</returns>
        public static With ContainsText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Argument is null or empty", nameof(text));
            }

            return Attribute(AttributeTags.TextContains, text);
        }

        /// <summary>
        /// Find node with matching resource id
        /// </summary>
        /// <param name="id">The resource if of the node to be found</param>
        /// <returns>An instance of the with object containing the search function</returns>
        public static With ResourceId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Argument is null or empty", nameof(id));
            }

            return Attribute(AttributeTags.ResourceId, id);
        }

        /// <summary>
        /// Find node with matching content desc
        /// </summary>
        /// <param name="contentDesc">The content desc of the node to be found</param>
        /// <returns>An instance of the with object containing the search function</returns>
        public static With ContentDesc(string contentDesc)
        {
            if (string.IsNullOrEmpty(contentDesc))
            {
                throw new ArgumentException("Argument is null or empty", nameof(contentDesc));
            }

            return Attribute(AttributeTags.ContentDesc, contentDesc);
        }

        /// <summary>
        /// Find node with matching class
        /// </summary>
        /// <param name="class">The class of the node to be found</param>
        /// <returns>An instance of the with object containing the search function</returns>
        public static With Class(string @class)
        {
            if (string.IsNullOrEmpty(@class))
            {
                throw new ArgumentException("Argument is null or empty", nameof(@class));
            }

            return Attribute(AttributeTags.Class, @class);
        }

        /// <summary>
        /// Find node with matching index
        /// </summary>
        /// <param name="index">The index of the node to be found</param>
        /// <returns>An instance of the with object containing the search function</returns>
        public static With Index(int index)
        {
            return Attribute(AttributeTags.Index, index.ToString());
        }

        /// <summary>
        /// Find node with matching package
        /// </summary>
        /// <param name="package">The package of the node to be found</param>
        /// <returns>An instance of the with objecting containing the search function</returns>
        public static With Package(string package)
        {
            return Attribute(AttributeTags.Package, package);
        }

        /// <summary>
        /// Find node that match the lambda expression
        /// </summary>
        /// <param name="predicate">The lambda expression</param>
        /// <returns>An instance of the with object containing the search function</returns>
        /// <code>
        /// device.Ui.CreateUiObject(With.Lambda(n => n.Text == "someText"));
        /// </code>
        public static With Lambda(Func<Node, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return new With(predicate, "complex lambda");
        }

        private static With Attribute(AttributeTags attribute, string value)
        {
            switch (attribute)
            {
                case AttributeTags.TextContains:
                    return new With(node => node.Text.Contains(value), $"text contains \"{value}\"");
                case AttributeTags.Text:
                    return new With(node => node.Text == value, $"text equals \"{value}\"");
                case AttributeTags.ResourceId:
                    return new With(node => node.ResourceId == value, $"resource id equals \"{value}\"");
                case AttributeTags.ContentDesc:
                    return new With(node => node.ContentDesc == value, $"content desc equals \"{value}\"");
                case AttributeTags.Class:
                    return new With(node => node.Class == value, $"class equals \"{value}\"");
                case AttributeTags.Index:
                    return new With(node => node.Index == value, $"index equals {value}");
                case AttributeTags.Package:
                    return new With(node => node.Package == value, $"package equals \"{value}\"");
                default:
                    throw new ArgumentOutOfRangeException(nameof(attribute), attribute, null);
            }
        }
    }
}
