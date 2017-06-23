using System;
using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Ui.Objects
{
    /// <summary>
    /// Provices the base class from which the classes that represent UI nodes are derived.
    /// </summary>
    public abstract class BaseUiObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUiObject"/> class.
        /// </summary>
        /// <param name="device">The current android device object.</param>
        /// <param name="withs">A set of <see cref="With">Withs</see> that tell us how we should find the UI object./></param>
        protected BaseUiObject(IAndroidDevice device, params With[] withs)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (withs == null || withs.Length == 0)
            {
                throw new ArgumentException("Argument is empty collection", nameof(withs));
            }

            Device = device;
            Withs = withs;
        }

        /// <summary>
        /// Gets all search criteria to find this ui object.
        /// </summary>
        internal With[] Withs { get; }

        /// <summary>
        /// Gets the current android device.
        /// </summary>
        protected IAndroidDevice Device { get; }

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
                    if ((DateTime.Now - startTime).Seconds > timeout)
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

        protected abstract IList<Node> TryFindNode(int timeout);
    }
}
