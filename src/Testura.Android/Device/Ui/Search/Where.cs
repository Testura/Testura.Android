using System;
using Testura.Android.Device.Ui.Nodes.Data;

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

        private Where(Func<Node, string, bool> nodeMatch, string errorMessage)
        {
            NodeMatch = nodeMatch;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the func used to find the node.
        /// </summary>
        public Func<Node, string, bool> NodeMatch { get; }

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

            return new Where(
                (node, wildcard) =>
                {
                    var searchValue = text.Replace(Wildcard, wildcard);
                    return node.Text != null && node.Text.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                },
                $"text equals \"{text}\"");
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

            return new Where(
                (node, wildcard) =>
                {
                    var searchValue = text.Replace(Wildcard, wildcard);
                    return node.Text != null && node.Text.ToLower().Contains(searchValue.ToLower());
                },
                $"text contains \"{text}\"");
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

            return new Where(
                (node, wildcard) =>
                {
                    var searchValue = id.Replace(Wildcard, wildcard);
                    return node.ResourceId != null && node.ResourceId.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                },
                $"resource id equals \"{id}\"");
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

            return new Where(
                (node, wildcard) =>
                {
                    var searchValue = contentDesc.Replace(Wildcard, wildcard);
                    return node.ContentDesc != null && node.ContentDesc.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                },
                $"content desc equals \"{contentDesc}\"");
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

            return new Where(
                (node, wildcard) =>
                {
                    var searchValue = @class.Replace(Wildcard, wildcard);
                    return node.Class != null && node.Class.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                },
                $"content desc equals \"{@class}\"");
        }

        /// <summary>
        /// Map to a node with matching index.
        /// </summary>
        /// <param name="index">The index of the node to be found.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        public static Where Index(string index)
        {
            return new Where((node, wildcard) => node.Index != null && node.Index.Equals(index, StringComparison.OrdinalIgnoreCase), $"index equals {index}");
        }

        /// <summary>
        /// Map to a node with matching package.
        /// </summary>
        /// <param name="package">The package of the node to be found.</param>
        /// <returns>An instance of the with objecting containing the search function.</returns>
        public static Where Package(string package)
        {
            if (string.IsNullOrEmpty(package))
            {
                throw new ArgumentException("Argument is null or empty", nameof(package));
            }

            return new Where(
                (node, wildcard) =>
                {
                    var searchValue = package.Replace(Wildcard, wildcard);
                    return node.Package != null && node.Package.Equals(searchValue, StringComparison.OrdinalIgnoreCase);
                },
                $"package equals \"{package}\"");
        }

        /// <summary>
        /// Map to node that match lambda expression.
        /// </summary>
        /// <param name="expression">The func used to map the node(s). The func takes current node, provided wildcard and return true if the node map otherwise false.</param>
        /// <param name="customErrorMessage">Error message if we can't find node.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        /// <code>
        /// device.Ui.CreateUiObject(With.Lambda(n => n.Text == "someText"));
        /// </code>
        public static Where Lambda(Func<Node, string, bool> expression, string customErrorMessage = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return new Where(expression, customErrorMessage ?? "complex lambda");
        }

        /// <summary>
        /// Map to node that match lambda expression.
        /// </summary>
        /// <param name="expression">The func used to map the node(s). The func takes current node and return true if the node map otherwise false.</param>
        /// <param name="customErrorMessage">Error message if we can't find node.</param>
        /// <returns>An instance of the with object containing the search function.</returns>
        /// <code>
        /// device.Ui.CreateUiObject(With.Lambda(n => n.Text == "someText"));
        /// </code>
        public static Where Lambda(Func<Node, bool> expression, string customErrorMessage = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return new Where((n, w) => expression(n), customErrorMessage ?? "complex lambda");
        }
    }
}