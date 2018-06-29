using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="bys">A set of <see cref="By">Withs</see> that tell us how we should find the UI object./></param>
        protected BaseUiObject(IList<By> bys)
        {
            if (bys == null || !bys.Any())
            {
                throw new ArgumentException("Argument is empty collection", nameof(bys));
            }

            Bys = bys;
        }

        /// <summary>
        /// Gets all search criteria to find this ui object.
        /// </summary>
        internal IList<By> Bys { get; }

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
        /// Find node(s) on the screen.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>All found node(s)</returns>
        protected abstract IList<Node> TryFindNode(TimeSpan timeout);
    }
}
