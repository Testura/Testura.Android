using System;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Util;

namespace Testura.Android.Device.Ui.Search
{
    /// <summary>
    /// Represent how we should find a node on the screen.
    /// </summary>
    public class Where
    {
        /// <summary>
        /// Wildcard that later can be replaced with a wildcard value.
        /// </summary>
        public const string Wildcard = "{{*}}";

        private Where(Func<Node, string, bool> nodeSearch, string errorMessage)
        {
            NodeSearch = nodeSearch;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the func used to find the node.
        /// </summary>
        public Func<Node, string, bool> NodeSearch { get; }

        /// <summary>
        /// Gets the error message that are thrown if we can't find the node.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Map to a node with matching text.
        /// </summary>
        /// <param name="text">The text of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static Where Text(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Argument is null or empty", nameof(text));
            }

            return Attribute(AttributeTag.Text, text);
        }

        /// <summary>
        /// Map to a node that contains text.
        /// </summary>
        /// <param name="text">The partial text of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static Where TextContains(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Argument is null or empty", nameof(text));
            }

            return Attribute(AttributeTag.TextContains, text);
        }

        /// <summary>
        /// Map to a node with matching resource id.
        /// </summary>
        /// <param name="id">The resource if of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static Where ResourceId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Argument is null or empty", nameof(id));
            }

            return Attribute(AttributeTag.ResourceId, id);
        }

        /// <summary>
        /// Map to a node with matching content desc.
        /// </summary>
        /// <param name="contentDesc">The content desc of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static Where ContentDesc(string contentDesc)
        {
            if (string.IsNullOrEmpty(contentDesc))
            {
                throw new ArgumentException("Argument is null or empty", nameof(contentDesc));
            }

            return Attribute(AttributeTag.ContentDesc, contentDesc);
        }

        /// <summary>
        /// Map to a node with matching class.
        /// </summary>
        /// <param name="class">The class of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static Where Class(string @class)
        {
            if (string.IsNullOrEmpty(@class))
            {
                throw new ArgumentException("Argument is null or empty", nameof(@class));
            }

            return Attribute(AttributeTag.Class, @class);
        }

        /// <summary>
        /// Map to a node with matching index.
        /// </summary>
        /// <param name="index">The index of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static Where Index(int index)
        {
            return Attribute(AttributeTag.Index, index.ToString());
        }

        /// <summary>
        /// Map to a node with matching package.
        /// </summary>
        /// <param name="package">The package of the node to be found.</param>
        /// <returns>An instance of the with objecting containing the search function.</returns>
        public static Where Package(string package)
        {
            return Attribute(AttributeTag.Package, package);
        }

        /// <summary>
        /// Map to node that match lambda expression.
        /// </summary>
        /// <param name="predicate">The lambda expression.</param>
        /// <param name="customErrorMessage">Error message if we can't find node.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        /// <code>
        /// device.Ui.CreateUiObject(With.Lambda(n => n.Text == "someText"));
        /// </code>
        public static Where Lambda(Func<Node, string, bool> predicate, string customErrorMessage = null)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return new Where(predicate, customErrorMessage ?? "complex lambda");
        }

        /// <summary>
        /// Map to node that match lambda expression.
        /// </summary>
        /// <param name="predicate">The lambda expression.</param>
        /// <param name="customErrorMessage">Error message if we can't find node.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        /// <code>
        /// device.Ui.CreateUiObject(With.Lambda(n => n.Text == "someText"));
        /// </code>
        public static Where Lambda(Func<Node, bool> predicate, string customErrorMessage = null)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return new Where((n, w) => predicate(n), customErrorMessage ?? "complex lambda");
        }

        private static Where Attribute(AttributeTag attribute, string value)
        {
            switch (attribute)
            {
                case AttributeTag.TextContains:
                    return new Where(
                        (node, wildcard) =>
                        {
                            var searchValue = value.Replace(Wildcard, wildcard);
                            return node.Text != null && node.Text.ToLower().Contains(searchValue.ToLower());
                        },
                        $"text contains \"{value}\"");

                case AttributeTag.Text:
                    return new Where(
                        (node, wildcard) =>
                        {
                            var searchValue = value.Replace(Wildcard, wildcard);
                            return node.Text != null && node.Text.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                        },
                        $"text equals \"{value}\"");

                case AttributeTag.ResourceId:
                    return new Where(
                        (node, wildcard) =>
                        {
                            var searchValue = value.Replace(Wildcard, wildcard);
                            return node.ResourceId != null && node.ResourceId.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                        },
                        $"resource id equals \"{value}\"");

                case AttributeTag.ContentDesc:
                    return new Where(
                        (node, wildcard) =>
                        {
                            var searchValue = value.Replace(Wildcard, wildcard);
                            return node.ContentDesc != null && node.ContentDesc.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                        },
                        $"content desc equals \"{value}\"");

                case AttributeTag.Class:
                    return new Where(
                        (node, wildcard) =>
                        {
                            var searchValue = value.Replace(Wildcard, wildcard);
                            return node.Class != null && node.Class.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                        },
                        $"class equals \"{value}\"");

                case AttributeTag.Index:
                    return new Where(
                        (node, wildcard) =>
                        {
                            var searchValue = value.Replace(Wildcard, wildcard);
                            return node.Index != null && node.Index.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                        },
                        $"index equals {value}");

                case AttributeTag.Package:
                    return new Where(
                        (node, wildcard) =>
                        {
                            var searchValue = value.Replace(Wildcard, wildcard);
                            return node.Package != null && node.Package.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                        },
                        $"package equals \"{value}\"");

                default:
                    throw new ArgumentOutOfRangeException(nameof(attribute), attribute, null);
            }
        }
    }
}