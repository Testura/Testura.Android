using System;
using System.Collections.Generic;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Ui.Objects
{
    public abstract class BaseUiObject
    {
        protected BaseUiObject(IAndroidDevice device, params With[] with)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (with == null || with.Length == 0)
            {
                throw new ArgumentException("Argument is empty collection", nameof(with));
            }

            Device = device;
            With = with;
        }

        /// <summary>
        /// Gets the current android device
        /// </summary>
        protected IAndroidDevice Device { get; }

        /// <summary>
        /// Gets all search criteria to find this ui object
        /// </summary>
        protected With[] With { get; }

        /// <summary>
        /// Wait for the node(s) to be visible
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>True if object is visible, otherwise false.</returns>
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
        /// Wait for the node(s) to be hidden
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>True if object is hidden, otherwise false</returns>
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
