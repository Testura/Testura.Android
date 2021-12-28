using System.Xml;
using System.Xml.Linq;
using Testura.Android.Device.Extensions;
using Testura.Android.Device.Server;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Logging;

namespace Testura.Android.Device.Services.Ui
{
    /// <summary>
    /// Provides functionality to get content on screen.
    /// </summary>
    public class UiService : INodeFinderService
    {
        private readonly IUiAutomatorServer _uiAutomatorServer;
        private readonly INodeParser _nodeParser;
        private readonly INodeFinder _nodeFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiService"/> class.
        /// </summary>
        /// <param name="uiAutomatorServer">The ui server used to get screen dumps</param>
        /// <param name="nodeParser">The node parser.</param>
        /// <param name="nodeFinder">The node finder.</param>
        public UiService(
            IUiAutomatorServer uiAutomatorServer,
            INodeParser nodeParser,
            INodeFinder nodeFinder)
        {
            _uiAutomatorServer = uiAutomatorServer;
            _nodeParser = nodeParser ?? throw new ArgumentNullException(nameof(nodeParser));
            _nodeFinder = nodeFinder ?? throw new ArgumentNullException(nameof(nodeFinder));
            Extensions = new List<IUiExtension>();
        }

        /// <summary>
        /// Gets the last screen dump
        /// </summary>
        public string LastScreenDump { get; private set; }

        /// <summary>
        /// Gets a list of ui extensions
        /// </summary>
        public IList<IUiExtension> Extensions { get; }

        /// <inheritdoc />
        public Node FindNode(IList<Where> wheres, TimeSpan timeout, string wildcard = null)
        {
            return FindNodes(wheres, timeout, wildcard).First();
        }

        /// <inheritdoc />
        public IList<Node> FindNodes(IList<Where> wheres, TimeSpan timeout, string wildcard = null)
        {
            if (wheres == null || !wheres.Any())
            {
                throw new ArgumentException("You must search with at least one \"where\"", nameof(wheres));
            }

            var startTime = DateTime.Now;
            while (true)
            {
                var nodes = AllNodes();
                foreach (var uiExtension in Extensions)
                {
                    if (uiExtension.CheckNodes(nodes))
                    {
                        startTime = DateTime.Now;
                    }
                }

                var approvedNodes = _nodeFinder.FindNodes(nodes, wheres, wildcard);
                if (approvedNodes.Any())
                {
                    return approvedNodes;
                }

                if ((DateTime.Now - startTime).TotalMilliseconds > timeout.TotalMilliseconds)
                {
                    DeviceLogger.Log("Failed to find node, last xml dump: ", DeviceLogger.LogLevel.Warning);
                    DeviceLogger.Log(LastScreenDump?.Replace(Environment.NewLine, string.Empty), DeviceLogger.LogLevel.Warning);
                    throw new UiNodeNotFoundException(wheres);
                }
            }
        }

        /// <summary>
        /// Get all nodes on the screen
        /// </summary>
        /// <returns>A list of all nodes on the screen</returns>
        public IList<Node> AllNodes()
        {
            try
            {
                LastScreenDump = _uiAutomatorServer.DumpUi();
                var parsedDump = XDocument.Parse(LastScreenDump);
                return _nodeParser.ParseNodes(parsedDump);
            }
            catch (XmlException ex)
            {
                throw new UiNodeNotFoundException("Could not parse nodes from dump", ex);
            }
        }
    }
}