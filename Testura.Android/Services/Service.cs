using Testura.Android.Device;

namespace Testura.Android.Services
{
    public abstract class Service
    {
        protected Service(IDeviceServices deviceServices)
        {
            DeviceServices = deviceServices;
        }

        protected Service()
        { 
        }

        protected IDeviceServices DeviceServices { get; private set; }

        public void InitializeDeviceServices(IDeviceServices deviceServices)
        {
            DeviceServices = deviceServices;
        }
    }
}
