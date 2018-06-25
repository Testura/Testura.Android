﻿using System;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Util;

namespace Testura.Android.Device.Ui.Search
{
    /// <summary>
    /// Represent how we should find a node on the screen.
    /// </summary>
    public class With
    {
        private With(Func<Node, bool> nodeSearch, string errorMessage)
        {
            NodeSearch = nodeSearch;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the func used to find the node.
        /// </summary>
        public Func<Node, bool> NodeSearch { get; }

        /// <summary>
        /// Gets the error message that are thrown if we can't find the node.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Find node with matching text.
        /// </summary>
        /// <param name="text">The text of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static With Text(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Argument is null or empty", nameof(text));
            }

            return Attribute(AttributeTag.Text, text);
        }

        /// <summary>
        /// Find node with text that contains text.
        /// </summary>
        /// <param name="text">The partial text of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static With ContainsText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Argument is null or empty", nameof(text));
            }

            return Attribute(AttributeTag.TextContains, text);
        }

        /// <summary>
        /// Find node with matching resource id.
        /// </summary>
        /// <param name="id">The resource if of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static With ResourceId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Argument is null or empty", nameof(id));
            }

            return Attribute(AttributeTag.ResourceId, id);
        }

        /// <summary>
        /// Find node with matching content desc.
        /// </summary>
        /// <param name="contentDesc">The content desc of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static With ContentDesc(string contentDesc)
        {
            if (string.IsNullOrEmpty(contentDesc))
            {
                throw new ArgumentException("Argument is null or empty", nameof(contentDesc));
            }

            return Attribute(AttributeTag.ContentDesc, contentDesc);
        }

        /// <summary>
        /// Find node with matching class.
        /// </summary>
        /// <param name="class">The class of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static With Class(string @class)
        {
            if (string.IsNullOrEmpty(@class))
            {
                throw new ArgumentException("Argument is null or empty", nameof(@class));
            }

            return Attribute(AttributeTag.Class, @class);
        }

        /// <summary>
        /// Find node with matching index.
        /// </summary>
        /// <param name="index">The index of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static With Index(int index)
        {
            return Attribute(AttributeTag.Index, index.ToString());
        }

        /// <summary>
        /// Find node with matching package.
        /// </summary>
        /// <param name="package">The package of the node to be found.</param>
        /// <returns>An instance of the with objecting containing the search function.</returns>
        public static With Package(string package)
        {
            return Attribute(AttributeTag.Package, package);
        }

        /// <summary>
        /// Find node that match the lambda expression.
        /// </summary>
        /// <param name="predicate">The lambda expression.</param>
        /// <param name="customErrorMessage">Error message if we can't find node.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        /// <code>
        /// device.Ui.CreateUiObject(With.Lambda(n => n.Text == "someText"));
        /// </code>
        public static With Lambda(Func<Node, bool> predicate, string customErrorMessage = null)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return new With(predicate, customErrorMessage ?? "complex lambda");
        }

        private static With Attribute(AttributeTag attribute, string value)
        {
            switch (attribute)
            {
                case AttributeTag.TextContains:
                    return new With(node => node.Text != null && node.Text.ToLower().Contains(value.ToLower()), $"text contains \"{value}\"");
                case AttributeTag.Text:
                    return new With(node => node.Text != null && node.Text.Equals(value, StringComparison.OrdinalIgnoreCase), $"text equals \"{value}\"");
                case AttributeTag.ResourceId:
                    return new With(node => node.ResourceId != null && node.ResourceId.Equals(value, StringComparison.OrdinalIgnoreCase), $"resource id equals \"{value}\"");
                case AttributeTag.ContentDesc:
                    return new With(node => node.ContentDesc != null && node.ContentDesc.Equals(value, StringComparison.OrdinalIgnoreCase), $"content desc equals \"{value}\"");
                case AttributeTag.Class:
                    return new With(node => node.Class != null && node.Class.Equals(value, StringComparison.OrdinalIgnoreCase), $"class equals \"{value}\"");
                case AttributeTag.Index:
                    return new With(node => node.Index != null && node.Index.Equals(value, StringComparison.OrdinalIgnoreCase), $"index equals {value}");
                case AttributeTag.Package:
                    return new With(node => node.Package != null && node.Package.Equals(value, StringComparison.OrdinalIgnoreCase), $"package equals \"{value}\"");
                default:
                    throw new ArgumentOutOfRangeException(nameof(attribute), attribute, null);
            }
        }
    }
}
