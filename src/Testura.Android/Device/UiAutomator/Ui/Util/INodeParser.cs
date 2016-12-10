using System.Collections.Generic;
using System.Xml.Linq;

namespace Testura.Android.Device.UiAutomator.Ui.Util
{
    public interface INodeParser
    {
        /// <summary>
        /// Parse all android nodes from an xmldocument
        /// </summary>
        /// <param name="screenDump">An xmldocument dump of the screen</param>
        /// <returns>A list with parsed nodes</returns>
        IList<Node> ParseNodes(XDocument screenDump);
    }
}