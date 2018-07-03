using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Helpers;

namespace Testura.Android.Device.Ui.Objects
{
    /// <summary>
    /// Represent a single UI Object on the screen
    /// </summary>
    public class UiObject
    {
        private readonly InteractionService _interactionService;
        private readonly INodeFinderService _nodeFinderService;
        private readonly string _wildcard;

        internal UiObject(InteractionService interactionService, INodeFinderService nodeFinderService, IList<Where> wheres, string wildcard)
        {
            if (wheres == null || !wheres.Any())
            {
                throw new ArgumentException("Argument is empty collection", nameof(wheres));
            }

            Wheres = wheres;
            _interactionService = interactionService;
            _nodeFinderService = nodeFinderService;
            _wildcard = wildcard;
        }

        /// <summary>
        /// Gets all mapping criterias
        /// </summary>
        internal IList<Where> Wheres { get; }

        /// <summary>
        /// Get a new ui object but replace all wild cards with the provided wildcard value
        /// </summary>
        /// <param name="wildcard">Wild card value</param>
        /// <returns>A new ui object but with the provided wildcard value</returns>
        /// <example>
        /// <code>
        /// var someObject = device.MapUiObject(Where.Text($"A value with {Where.Wildcard}"));
        /// var someObjectWithWildcardValue = someObject["MyWildcardValue"]; //Will now have a Where.text with "A value with MyWildcardValue"
        /// </code>
        /// </example>
        public UiObject this[string wildcard] => new UiObject(_interactionService, _nodeFinderService, Wheres, wildcard);

        /// <summary>
        /// Wait for the node(s) to be visible.
        /// </summary>
        /// <returns><c>true</c> if object is visible, otherwise <c>false</c>.</returns>
        public bool IsVisible()
        {
            return IsVisible(TimeSpan.FromSeconds(20));
        }

        /// <summary>
        /// Wait for the node(s) to be visible.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns><c>true</c> if object is visible, otherwise <c>false</c>.</returns>
        public bool IsVisible(TimeSpan timeout)
        {
            try
            {
                TryFindNode(timeout);
                return true;
            }
            catch (UiNodeNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Wait for the node(s) to be hidden.
        /// </summary>
        /// <returns><c>true</c> if object is hidden, otherwise <c>false</c></returns>
        public bool IsHidden()
        {
            return IsHidden(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Wait for the node(s) to be hidden.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns><c>true</c> if object is hidden, otherwise <c>false</c></returns>
        public bool IsHidden(TimeSpan timeout)
        {
            var startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    TryFindNode(TimeSpan.FromSeconds(1));
                    if ((DateTime.Now - startTime).TotalMilliseconds > timeout.TotalMilliseconds)
                    {
                        return false;
                    }
                }
                catch (UiNodeNotFoundException)
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Tap in the center of the first node that match all our predicates.
        /// </summary>
        public void Tap()
        {
            Tap(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Tap in the center of the first node that match all our predicates.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public void Tap(TimeSpan timeout)
        {
            var node = _nodeFinderService.FindNode(Wheres, timeout, _wildcard);
            _interactionService.Tap(node);
        }

        /// <summary>
        /// Input text into the node that match all our predicates.
        /// </summary>
        /// <param name="text">The text to input into the nod.e</param>
        public void InputText(string text)
        {
            InputText(text, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Input text into the the first node that match all our predicates.
        /// </summary>
        /// <param name="text">The text to input into the nod.e</param>
        /// <param name="timeout">Timeout</param>
        public void InputText(string text, TimeSpan timeout)
        {
            Tap(timeout);
            var tries = 0;
            while (!WaitUntil(n => n.Focused, TimeSpan.FromSeconds(1)) && tries < 3)
            {
                Tap(TimeSpan.FromSeconds(1));
                tries++;
            }

            _interactionService.InputText(text);
        }

        /// <summary>
        /// Get first node that match all our predicates.
        /// </summary>
        /// <returns>The first node that match all our predicates</returns>
        public Node First()
        {
            return First(TimeSpan.FromSeconds(20));
        }

        /// <summary>
        /// Get first node that match all our predicates.
        /// </summary>
        /// <param name="timeout">How long we should wait before we stop looking for a node</param>
        /// <returns>The first node that match all our predicates</returns>
        public Node First(TimeSpan timeout)
        {
            return TryFindNode(timeout).First();
        }

        /// <summary>
        /// Get all nodes that match all our predicates.
        /// </summary>
        /// <returns>All nodes that match our predicates.</returns>
        public List<Node> All()
        {
            return All(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Get all nodes that match all our predicates.
        /// </summary>
        /// <param name="timeout">How long we should wait before we stop looking for a node</param>
        /// <returns>A node object with all values of this node.</returns>
        public List<Node> All(TimeSpan timeout)
        {
            return TryFindNode(timeout).ToList();
        }

        /// <summary>
        /// Get the first node that match all our predicates and wait until it match all expected values.
        /// </summary>
        /// <param name="expectedValues">Expected values on the node.</param>
        /// <returns>True if node values match before timeout, otherwise false.</returns>
        public bool WaitUntil(Func<Node, bool> expectedValues)
        {
            return WaitUntil(expectedValues, TimeSpan.FromSeconds(20));
        }

        /// <summary>
        /// Get the first node that match all our predicates and wait until it match all expected values.
        /// </summary>
        /// <param name="expectedValues">Expected values on the node.</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>True if node values match before timeout, otherwise false.</returns>
        public bool WaitUntil(Func<Node, bool> expectedValues, TimeSpan timeout)
        {
            var startTime = DateTime.Now;
            while (true)
            {
                var currentValues = All(timeout).First();

                if (currentValues != null && expectedValues.Invoke(currentValues))
                {
                    return true;
                }

                if ((DateTime.Now - startTime).TotalMilliseconds > timeout.TotalMilliseconds)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Find the closest node(based on the first node that match all our predicates) that matches specific where(s)
        /// </summary>
        /// <param name="wheres">"Where(s)" that the close node should match</param>
        /// <returns>The closest node, null if we can't find a matching node</returns>
        public Node FindClosestNode(params Where[] wheres)
        {
            var node = First();
            return UiFind.ClosestNode(node, wheres);
        }

        /// <summary>
        /// Find node(s) on the screen.
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>All found node(s)</returns>
        protected IList<Node> TryFindNode(TimeSpan timeout)
        {
            return _nodeFinderService.FindNodes(Wheres, timeout, _wildcard);
        }
    }
}