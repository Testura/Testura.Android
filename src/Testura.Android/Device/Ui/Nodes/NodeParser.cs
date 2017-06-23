using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Testura.Android.Device.Ui.Nodes.Data;

namespace Testura.Android.Device.Ui.Nodes
{
    /// <summary>
    /// Provides functionality to parse nodes from an xml document.
    /// </summary>
    public class NodeParser : INodeParser
    {
        /// <summary>
        /// Parse all android nodes from an xmldocument.
        /// </summary>
        /// <param name="screenDump">An xmldocument dump of the screen.</param>
        /// <returns>A list with parsed nodes.</returns>
        public IList<Node> ParseNodes(XDocument screenDump)
        {
            if (screenDump == null)
            {
                throw new ArgumentNullException(nameof(screenDump));
            }

            var nodes = new List<Node>();
            foreach (var element in screenDump.Descendants("node"))
            {
                Node node;
                var parent = nodes.FirstOrDefault(p => p.Element == element.Parent);
                if (parent != null)
                {
                    node = new Node(element, parent);
                    parent.Children.Add(node);
                }
                else
                {
                    node = new Node(element, null);
                }

                nodes.Add(node);
            }

            return nodes;
        }
    }
}
