using System;

namespace Testura.Android.Device.Services
{
    public abstract class Service
    {
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
