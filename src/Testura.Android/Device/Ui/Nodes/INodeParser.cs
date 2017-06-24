using System.Collections.Generic;
using System.Xml.Linq;
using Testura.Android.Device.Ui.Nodes.Data;

namespace Testura.Android.Device.Ui.Nodes
{
    /// <summary>
    /// Defines methods to parse nodes from an xml document.
    /// </summary>
    public interface INodeParser
    {
        /// <summary>
        /// Parse all android nodes from an xmldocument.
        /// </summary>
        /// <param name="screenDump">An xmldocument dump of the screen.</param>
        /// <returns>A list with parsed nodes.</returns>
        IList<Node> ParseNodes(XDocument screenDump);
    }
}