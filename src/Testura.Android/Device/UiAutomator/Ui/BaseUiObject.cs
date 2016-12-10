using System;
using System.Collections.Generic;
using Testura.Android.Device.UiAutomator.Ui.Search;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.UiAutomator.Ui
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
        /// Gets all search criterias to find this ui object
        /// </summary>
        protected With[] With { get; }

        /// <summary>
        /// Wait for the node(s) to be visible
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="continueIfFalse">If true we continue and return false even if we can't find object, otherwise we throw exception</param>
        /// <returns>True if we can find object, otherwise false (if continueIfFalse is true)</returns>
        public bool IsVisible(int timeout = 10, bool continueIfFalse = false)
        {
            try
            {
                TryFindNode(timeout);
                return true;
            }
            catch (UiNodeNotFoundException)
            {
                if (continueIfFalse)
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Wait for the node(s) to be hidden
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="continueIfFalse">If true we continue and return false even if we can find object, otherwise we throw exception</param>
        /// <returns>True if we can't find object, otherwise false (if continueIfFalse is true)</returns>
        public bool IsHidden(int timeout = 10, bool continueIfFalse = false)
        {
            var startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    TryFindNode(1);
                    if ((DateTime.Now - startTime).Seconds > timeout)
                    {
                        if (continueIfFalse)
                        {
                            return false;
                        }

                        throw new UiNodeNotFoundException(null);
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
