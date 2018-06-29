using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Util.Helpers
{
    /// <summary>
    /// Provides the functionality to wait for multiple UI Objects at the same time.
    /// </summary>
    public static class UiWait
    {
        /// <summary>
        /// Wait for any ui object to be visible or hidden.
        /// </summary>
        /// <param name="invokeMethods">Methods on different uiObjects to invoke (for example Visible or Hidden)</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>The first uiObject to be visible or hidden</returns>
        /// <example>This example show how to call the method with two uiObjects and wait for the
        /// first one to be visible or the second one to be hidden.
        /// </example>
        /// <code>
        /// UiWait.ForAny(10, uiObjectOne.Visible, uiObjectTwo.Hidden)
        /// </code>
        /// <exception cref="UiNodeNotFoundException">Thrown If no ui objects are visible/hidden</exception>
        public static UiObject ForAny(Func<TimeSpan, bool>[] invokeMethods, TimeSpan timeout)
        {
            if (invokeMethods == null)
            {
                throw new ArgumentNullException(nameof(invokeMethods));
            }

            if (invokeMethods.Length == 0)
            {
                throw new ArgumentException("Argument is empty collection", nameof(invokeMethods));
            }

            var tasks = invokeMethods.Select(executeMethod => Task.Run(() =>
            {
                try
                {
                    return executeMethod.Invoke(timeout);
                }
                catch (UiNodeNotFoundException)
                {
                    return false;
                }
            })).ToArray();
            var index = Task.WaitAny(tasks);
            var task = tasks[index];
            if (!task.Result || task.Exception != null)
            {
                throw new UiNodeNotFoundException("Could not find any of the provided ui objects");
            }

            return (UiObject)invokeMethods[index].Target;
        }

        /// <summary>
        /// Wait for all ui object to be visible or hidden.
        /// </summary>
        /// <param name="invokeMethods">Methods on different uiObjects to invoke (for example Visible or Hidden)</param>
        /// <param name="timeout">Timeout</param>
        /// <example>This example show how to call the method with two uiObjects and wait for the
        /// first one to be visible or the second one to be hidden.
        /// </example>
        /// <code>
        /// UiWait.ForAll(10, uiObjectOne.Visible, uiObjectTwo.Hidden)
        /// </code>
        /// <exception cref="UiNodeNotFoundException">Thrown If not all ui objects are visible/hidden</exception>
        public static void ForAll(Func<TimeSpan, bool>[] invokeMethods, TimeSpan timeout)
        {
            if (invokeMethods == null)
            {
                throw new ArgumentNullException(nameof(invokeMethods));
            }

            if (invokeMethods.Length == 0)
            {
                throw new ArgumentException("Argument is empty collection", nameof(invokeMethods));
            }

            var invokeMethodByTask = new Dictionary<Task<bool>, Func<TimeSpan, bool>>();

            foreach (var invokeMethod in invokeMethods)
            {
                var task = Task.Run(() =>
                {
                    try
                    {
                        return invokeMethod.Invoke(timeout);
                    }
                    catch (UiNodeNotFoundException)
                    {
                        return false;
                    }
                });
                invokeMethodByTask.Add(task, invokeMethod);
            }

            Task.WaitAll(invokeMethodByTask.Keys.ToArray());

            var failedTasks = invokeMethodByTask.Keys.Where(t => t.Result == false || t.Exception != null);
            if (failedTasks.Any())
            {
                BuildError(invokeMethodByTask, failedTasks);
            }
        }

        private static void BuildError(IReadOnlyDictionary<Task<bool>, Func<TimeSpan, bool>> invokeMethodByTask, IEnumerable<Task<bool>> failedTasks)
        {
            var errorMessage = new StringBuilder();
            foreach (var notCompletedTask in failedTasks)
            {
                if (invokeMethodByTask[notCompletedTask].Target is UiObject uiObject)
                {
                    errorMessage.Append($"{ByErrorMessageBuilder.BuildByErrorMessage(uiObject.Bys).Replace("Could not find node where", "Node where")} \r\n");
                }
            }

            throw new UiNodeNotFoundException("These nodes where not as expected (visible or hidden): \r\n \r\n" + errorMessage);
        }
    }
}
