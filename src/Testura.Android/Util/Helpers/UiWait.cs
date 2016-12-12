using System;
using System.Linq;
using System.Threading.Tasks;
using Testura.Android.Device.UiAutomator.Ui;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Util.Helpers
{
    public static class UiWait
    {
        /// <summary>
        /// Wait for any ui object to be visible or hidden.
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="invokeMethods">Methods on different uiObjects to invoke (for example Visible or Hidden)</param>
        /// <returns>The first uiObject to be visible or hidden</returns>
        /// <example>This example show how to call the method with two uiObjects and wait for the
        /// first one to be visible or the second one to be hidden.
        /// </example>
        /// <code>
        /// UiWait.ForAny(10, uiObjectOne.Visible, uiObjectTwo.Hidden)
        /// </code>
        /// <exception cref="UiNodeNotFoundException">Thrown If no ui objects are visible/hidden</exception>
        public static BaseUiObject ForAny(int timeout, params Func<int, BaseUiObject>[] invokeMethods)
        {
            var tasks = invokeMethods.Select(executeMethod => Task.Run(() =>
            {
                try
                {
                    return executeMethod.Invoke(timeout);
                }
                catch (UiNodeNotFoundException)
                {
                    return null;
                }
            })).ToArray();
            var result = tasks[Task.WaitAny(tasks)].Result;
            if (result == null)
            {
                throw new UiNodeNotFoundException("Could not find any of the provided ui objects");
            }
            return result;
        }
    }
}
