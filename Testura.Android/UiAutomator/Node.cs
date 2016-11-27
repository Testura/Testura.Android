using System.Collections.Generic;
using System.Xml.Linq;
using Testura.Android.Util;

namespace Testura.Android.UiAutomator
{
    public class Node
    {
        private readonly XElement node;

        public Node(XElement node)
        {
            this.node = node;
            Text = node.Attribute("text")?.Value;
        }

        public string Text { get; }

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
        /// <returns>A list with the top left and lower right coordinate´.</returns>
        private List<Coordinate> GetNodeBounds()
        {
            var bounds = node.Attribute("bounds");

            // Could we use regexp? Yes, but this is more hardcore.
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
    }
}
