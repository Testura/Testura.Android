using System;

namespace Testura.Android.Device.Services
{
    /// <summary>
    /// Provides the base class from which the class that represent device services are derived.
    /// </summary>
    public abstract class Service
    {
        /// <summary>
        /// Gets the device.
        /// </summary>
        protected IAndroidDevice Device { get; private set; }

        /// <summary>
        /// Set the owner of the service
        /// </summary>
        /// <param name="device">Owner of the service</param>
        public void InitializeServiceOwner(IAndroidDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            Device = device;
        }
    }
}
