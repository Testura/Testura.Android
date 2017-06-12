using System.Reflection;
using Testura.Android.Device;
using Testura.Android.PageObject.Attributes;

namespace Testura.Android.PageObject
{
    /// <summary>
    /// This class represent a view in the application
    /// </summary>
    public abstract class View
    {
        protected View(IAndroidDevice device)
        {
            Device = device;
            InitializeUiObjects();
        }

        /// <summary>
        /// Gets or sets the current android device
        /// </summary>
        protected IAndroidDevice Device { get; set; }

        /// <summary>
        /// Go through all properties and fields and check for those that implement the
        /// InitializeUiObjectAttribute and then initialize them.
        /// </summary>
        private void InitializeUiObjects()
        {
            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(CreateAttribute), true);
                if (attributes.Length >= 1)
                {
                    var attribute = attributes[0] as CreateAttribute;
                    property.SetValue(this, Device.Ui.CreateUiObject(attribute.With));
                }
            }

            var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(typeof(CreateAttribute), true);
                if (attributes.Length >= 1)
                {
                    var attribute = attributes[0] as CreateAttribute;
                    field.SetValue(this, Device.Ui.CreateUiObject(attribute.With));
                }
            }
        }
    }
}
