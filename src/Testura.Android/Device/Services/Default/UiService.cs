using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device.Extensions;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Services.Default
{
    /// <summary>
    /// Provides functionality to get content on screen.
    /// </summary>
    public class UiService : Service, IUiService
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
        public UiService(IScreenDumper screenDumper, INodeParser nodeParser, INodeFinder nodeFinder)
        {
            if (screenDumper == null)
            {
                throw new ArgumentNullException(nameof(screenDumper));
            }

            if (nodeParser == null)
            {
                throw new ArgumentNullException(nameof(nodeParser));
            }

            if (nodeFinder == null)
            {
                throw new ArgumentNullException(nameof(nodeFinder));
            }

            _screenDumper = screenDumper;
            _nodeParser = nodeParser;
            _nodeFinder = nodeFinder;
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
                    var nodes = GetAllNodesOnScreen();
                    foreach (var uiExtension in Extensions)
                    {
                        if (uiExtension.CheckNodes(nodes, Device))
                        {
                            startTime = DateTime.Now;
                        }
                    }

                    return _nodeFinder.FindNodes(nodes, with);
                }
                catch (UiNodeNotFoundException)
                {
                    if ((DateTime.Now - startTime).Seconds > timeout)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Create a new ui object that wraps around a node that match a specific search criteria
        /// </summary>
        /// <param name="with">Find node with</param>
        /// <returns>The mapped ui object</returns>
        public UiObject CreateUiObject(params With[] with)
        {
            return new UiObject(Device, with.ToArray());
        }

        /// <summary>
        /// Create a new ui object that wraps around multiple nodes that match a specific search criteria
        /// </summary>
        /// <param name="with">Find nodes with</param>
        /// <returns>The mapped ui object</returns>
        public UiObjects CreateUiObjects(params With[] with)
        {
            return new UiObjects(Device, with.ToArray());
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

        /// <summary>
        /// Get all nodes on the screen
        /// </summary>
        /// <returns>A list with all nodes on the screen</returns>
        internal IList<Node> GetAllNodesOnScreen()
        {
            var screenDump = _screenDumper.DumpUi();
            return _nodeParser.ParseNodes(screenDump);
        }
    }
}