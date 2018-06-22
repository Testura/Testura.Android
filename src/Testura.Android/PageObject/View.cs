using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device;

namespace Testura.Android.PageObject
{
    /// <summary>
    /// Represent a view in a android application.
    /// </summary>
    public abstract class View
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="View"/> class.
        /// </summary>
        /// <param name="device">Current android device</param>
        protected View(IAndroidDevice device)
        {
            Device = device;
            ViewFactory.MapUiNodes(device, this);
        }

        /// <summary>
        /// Gets or sets the current android device
        /// </summary>
        protected IAndroidDevice Device { get; set; }

        protected void Validate<T>(params IAndroidViewValidation<T>[] validators)
            where T : View
        {
            var exceptions = new List<Exception>();
            foreach (var validator in validators)
            {
                try
                {
                    validator.Validate(Device, (T)this);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

    }
}
