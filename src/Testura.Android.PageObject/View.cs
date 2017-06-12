using Testura.Android.Device;

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
                var attributes = property.GetCustomAttributes(typeof(InitializeUiObjectAttribute), true);
                if (attributes.Length >= 1)
                {
                    var attribute = attributes[0] as InitializeUiObjectAttribute;
                    property.SetValue(this, attribute.With);
                }
            }

            var fields = GetType().GetFields();
            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(typeof(InitializeUiObjectAttribute), true);
                if (attributes.Length >= 1)
                {
                    var attribute = attributes[0] as InitializeUiObjectAttribute;
                    field.SetValue(this, attribute.With);
                }
            }
        }
    }
}
