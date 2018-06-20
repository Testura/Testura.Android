using System;
using System.Collections.Generic;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Ui.Objects
{
    /// <summary>
    /// Provides the base class from which the classes that represent UI nodes are derived.
    /// </summary>
    public abstract class BaseUiObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUiObject"/> class.
        /// </summary>
        /// <param name="device">The current android device object.</param>
        /// <param name="withs">A set of <see cref="With">Withs</see> that tell us how we should find the UI object./></param>
        protected BaseUiObject(params With[] withs)
        {
            if (withs == null || withs.Length == 0)
            {
                throw new ArgumentException("Argument is empty collection", nameof(withs));
            }

            Withs = withs;
        }

        /// Gets all search criteria to find this ui object.
        /// </summary>
        internal With[] Withs { get; }

        /// <summary>
        /// Wait for the node(s) to be visible.
        /// </summary>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns><c>true</c> if object is visible, otherwise <c>false</c>.</returns>
        public bool IsVisible(int timeout = 10)
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
        /// Check if node(s) is visible in cache.
        /// </summary>
        /// <returns>True if visible in cache, otherwise false.</returns>
        public bool IsVisibleInCache()
        {
            return IsVisible(-1);
        }

        /// <summary>
        /// Wait for the node(s) to be hidden.
        /// </summary>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns><c>true</c> if object is hidden, otherwise <c>false</c></returns>
        public bool IsHidden(int timeout = 10)
        {
            var startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    TryFindNode(1);
                    if ((DateTime.Now - startTime).TotalSeconds > timeout)
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
        /// Check if node(s) is hidden in cache.
        /// </summary>
        /// <returns>True if hidden, otherwise false.</returns>
        public bool IsHiddenInCache()
        {
            try
            {
                TryFindNode(-1);
                return false;
            }
            catch (UiNodeNotFoundException)
            {
                return true;
            }
        }

        /// <summary>
        /// Find node(s) on the screen.
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>All found node(s)</returns>
        protected abstract IList<Node> TryFindNode(int timeout);
    }
}
