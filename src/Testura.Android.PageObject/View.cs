using System;
using System.Reflection;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.PageObject.Attributes;

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
            InitializeUiObjectsFromProperties(GetType());
            InitializeUiObjectsFromFields(GetType());
        }

        private void InitializeUiObjectsFromProperties(Type type)
        {
            if (type.BaseType != null)
            {
                InitializeUiObjectsFromProperties(type.BaseType);
            }

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType != typeof(UiObject) && property.PropertyType != typeof(UiObjects))
                {
                    continue;
                }

                var attributes = property.GetCustomAttributes(typeof(CreateAttribute), true);
                if (attributes.Length >= 1)
                {
                    var attribute = attributes[0] as CreateAttribute;
                    if (property.PropertyType == typeof(UiObject))
                    {
                        property.SetValue(this, Device.Ui.CreateUiObject(attribute.With));
                    }
                    else
                    {
                        property.SetValue(this, Device.Ui.CreateUiObjects(attribute.With));
                    }
                }
            }
        }

        private void InitializeUiObjectsFromFields(Type type)
        {
            if (type.BaseType != null)
            {
                InitializeUiObjectsFromFields(type.BaseType);
            }

            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.FieldType != typeof(UiObject) && field.FieldType != typeof(UiObjects))
                {
                    continue;
                }

                var attributes = field.GetCustomAttributes(typeof(CreateAttribute), true);
                if (attributes.Length >= 1)
                {
                    var attribute = attributes[0] as CreateAttribute;
                    if (field.FieldType == typeof(UiObject))
                    {
                        field.SetValue(this, Device.Ui.CreateUiObject(attribute.With));
                    }
                    else
                    {
                        field.SetValue(this, Device.Ui.CreateUiObjects(attribute.With));
                    }
                }
            }
        }
    }
}
