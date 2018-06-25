using System;
using System.Collections.Generic;
using System.Linq;
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
        private string _lastScreenDump;

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
        /// Gets a list of ui extensions
        /// </summary>
        public IList<IUiExtension> Extensions { get; }

        /// <summary>
        /// Find a node on the screen
        /// </summary>
        /// <param name="withs">Find node with</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>Returns found node</returns>
        /// <exception cref="UiNodeNotFoundException">If we timeout and can't find the node</exception>
        public Node FindNode(IList<With> withs, TimeSpan timeout)
        {
            return FindNodes(withs, timeout).First();
        }

        /// <summary>
        /// Find multiple nodes on the screen
        /// </summary>
        /// <param name="withs">Find node with</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>Returns found node</returns>
        /// <exception cref="UiNodeNotFoundException">If we timeout and can't find any nodes</exception>
        public IList<Node> FindNodes(IList<With> withs, TimeSpan timeout)
        {
            if (withs == null || !withs.Any())
            {
                throw new ArgumentException("You must search with at least one \"with\"", nameof(withs));
            }

            var startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    var nodes = AllNodes();
                    foreach (var uiExtension in Extensions)
                    {
                        if (uiExtension.CheckNodes(nodes))
                        {
                            startTime = DateTime.Now;
                        }
                    }

                    return _nodeFinder.FindNodes(nodes, withs);
                }
                catch (UiNodeNotFoundException)
                {
                    if ((DateTime.Now - startTime).TotalMilliseconds > timeout.TotalMilliseconds)
                    {
                        DeviceLogger.Log("Failed to find node, last xml dump: ", DeviceLogger.LogLevel.Warning);
                        DeviceLogger.Log(_lastScreenDump?.Replace(Environment.NewLine, "replacement text"), DeviceLogger.LogLevel.Warning);
                        throw;
                    }
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
                _lastScreenDump = _uiAutomatorServer.DumpUi();
                var parsedDump = XDocument.Parse(_lastScreenDump);
                return _nodeParser.ParseNodes(parsedDump);
            }
            catch (XmlException ex)
            {
                throw new UiNodeNotFoundException("Could not parse nodes from dump", ex);
            }
        }
    }
}