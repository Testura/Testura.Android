using System.Linq;
using Testura.Android.Services;

namespace Testura.Android.Device
{
    public class AndroidDevice : IDeviceServices
    {
        public AndroidDevice(IAdbService adb, IUiService ui)
        {
            Adb = adb;
            Ui = ui;
            InitializeServices();
        }

        public IAdbService Adb { get; }

        public IUiService Ui { get; }

        private void InitializeServices()
        {
            var services =
                typeof(AndroidDevice).GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof (Service)));
            foreach (var propertyInfo in services)
            {
                ((Service)propertyInfo.GetValue(this)).InitializeDeviceServices(this);
            }
        }
    }
}
