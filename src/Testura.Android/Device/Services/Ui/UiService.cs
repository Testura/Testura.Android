using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device.Extensions;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Services.Ui
{
    /// <summary>
    /// Provides functionality to get content on screen.
    /// </summary>
    public class UiService : INodeFinderService
    {
        private readonly IScreenDumper _screenDumper;
        private readonly INodeParser _nodeParser;
        private readonly INodeFinder _nodeFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiService"/> class.
        /// </summary>
        /// <param name="screenDumper">The screen dumper.</param>
        /// <param name="nodeParser">The node parser.</param>
        /// <param name="nodeFinder">The node finder.</param>
        public UiService(
            IScreenDumper screenDumper,
            INodeParser nodeParser,
            INodeFinder nodeFinder)
        {
            _screenDumper = screenDumper ?? throw new ArgumentNullException(nameof(screenDumper));
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
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="with">Find node with</param>
        /// <returns>Returns found node</returns>
        /// <exception cref="UiNodeNotFoundException">If we timeout and can't find the node</exception>
        public Node FindNode(int timeout, params With[] with)
        {
            return FindNodes(timeout, with).First();
        }

        /// <summary>
        /// Find multiple nodes on the screen
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="with">Find node with</param>
        /// <returns>Returns found node</returns>
        /// <exception cref="UiNodeNotFoundException">If we timeout and can't find any nodes</exception>
        public IList<Node> FindNodes(int timeout, With[] with)
        {
            if (with == null || !with.Any())
            {
                throw new ArgumentException("You must search with at least one \"with\"", nameof(with));
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

                    return _nodeFinder.FindNodes(nodes, with);
                }
                catch (UiNodeNotFoundException)
                {
                    if ((DateTime.Now - startTime).TotalSeconds > timeout)
                    {
                        throw;
                    }
                }
            }
        }

        public IList<Node> AllNodes()
        {
            return _nodeParser.ParseNodes(_screenDumper.DumpUi());
        }

        /// <summary>
        /// Start the ui server
        /// </summary>
        public void StartUiServer()
        {
            _screenDumper.StartUiServer();
        }

        /// <summary>
        /// Stop the ui server
        /// </summary>
        public void StopUiServer()
        {
            _screenDumper.StopUiServer();
        }
    }
}