using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Testura.Android.Util;

namespace Testura.Android.Device.Ui.Nodes.Data
{
    /// <summary>
    /// Represent a node in the UI hierarchy
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="element">The xml element containing all information about the node</param>
        /// <param name="parent">Node parent to this node</param>
        public Node(XElement element, Node parent)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
            Parent = parent;
            Children = new List<Node>();
            Index = element.Attribute("index")?.Value;
            Text = element.Attribute("text")?.Value;
            ResourceId = element.Attribute("resource-id")?.Value;
            Class = element.Attribute("class")?.Value;
            Package = element.Attribute("package")?.Value;
            ContentDesc = element.Attribute("content-desc")?.Value;
            Checkable = ParseAttribute("checkable");
            Checked = ParseAttribute("checked");
            Clickable = ParseAttribute("clickable");
            Enabled = ParseAttribute("enabled");
            Focusable = ParseAttribute("focusable");
            Focused = ParseAttribute("focused");
            Scrollable = ParseAttribute("scrollable");
            LongClickable = ParseAttribute("long-clickable");
            Password = ParseAttribute("password");
            Selected = ParseAttribute("selected");
        }

        /// <summary>
        /// Gets the raw element of a node.
        /// </summary>
        public XElement Element { get; }

        /// <summary>
        /// Gets the parent of a node.
        /// </summary>
        public Node Parent { get; }

        /// <summary>
        /// Gets all children of a node.
        /// </summary>
        public IList<Node> Children { get; }

        /// <summary>
        /// Gets the text of a node.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the resource if of a node.
        /// </summary>
        public string ResourceId { get; }

        /// <summary>
        /// Gets the content desc of a node.
        /// </summary>
        public string ContentDesc { get; }

        /// <summary>
        /// Gets the class of a node.
        /// </summary>
        public string Class { get; }

        /// <summary>
        /// Gets the index of a node.
        /// </summary>
        public string Index { get; }

        /// <summary>
        /// Gets the package of the node.
        /// </summary>
        public string Package { get; }

        /// <summary>
        /// Gets a value indicating whether the node is checkable.
        /// </summary>
        public bool Checkable { get; }

        /// <summary>
        /// Gets a value indicating whether the node is checked.
        /// </summary>
        public bool Checked { get; }

        /// <summary>
        /// Gets a value indicating whether the node is clickable.
        /// </summary>
        public bool Clickable { get; }

        /// <summary>
        /// Gets a value indicating whether the node is enabled.
        /// </summary>
        public bool Enabled { get; }

        /// <summary>
        /// Gets a value indicating whether the node is focusabled.
        /// </summary>
        public bool Focusable { get; }

        /// <summary>
        /// Gets a value indicating whether the node is focused.
        /// </summary>
        public bool Focused { get; }

        /// <summary>
        /// Gets a value indicating whether the node is scrollable.
        /// </summary>
        public bool Scrollable { get; }

        /// <summary>
        /// Gets a value indicating whether the node is long clickable.
        /// </summary>
        public bool LongClickable { get; }

        /// <summary>
        /// Gets a value indicating whether the node is a password.
        /// </summary>
        public bool Password { get; }

        /// <summary>
        /// Gets a value indicating whether the node is selected.
        /// </summary>
        public bool Selected { get; }

        /// <summary>
        /// Get coordinates of the node center point.
        /// </summary>
        /// <returns>Coordinates in the center of a node.</returns>
        public Coordinate GetNodeCenter()
        {
            var bounds = GetNodeBounds();
            var width = bounds[1].X - bounds[0].X;
            var height = bounds[1].Y - bounds[0].Y;
            return new Coordinate(bounds[0].X + (width / 2), bounds[0].Y + (height / 2));
        }

        /// <summary>
        /// Get the top left and lower right corner of a nod.
        /// </summary>
        /// <returns>A list with the top left and lower right coordinate.</returns>
        public List<Coordinate> GetNodeBounds()
        {
            var bounds = Element.Attribute("bounds");

            // Could we use regexp? Yes, but this is more hardcore.
            if (bounds == null)
            {
                return new List<Coordinate>();
            }

            var values = bounds.Value
                .Replace("][", ",")
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Split(',');

            return new List<Coordinate>
            {
                new Coordinate(int.Parse(values[0]), int.Parse(values[1])),
                new Coordinate(int.Parse(values[2]), int.Parse(values[3]))
            };
        }

        private bool ParseAttribute(string name)
        {
            bool.TryParse(Element.Attribute(name)?.Value, out var value);
            return value;
        }
    }
}
