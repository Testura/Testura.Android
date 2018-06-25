﻿using System;
using System.Linq;
using System.Reflection;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.PageObject.Attributes;

namespace Testura.Android.PageObject
{
    /// <summary>
    /// Provides the functionality to map ui nodes through the "UiMap" attribute.
    /// </summary>
    public static class ViewFactory
    {
        /// <summary>
        /// Go through all fields/properties in the provided object and look for those
        /// that have the "UiMap" attribute and initialze them.
        /// </summary>
        /// <param name="mapper">The mapper used to map the UI objects.</param>
        /// <param name="obj">Object to initialize</param>
        public static void MapUiNodes(IAndroidUiMapper mapper, object obj)
        {
            MapUiObjectsFromProperties(obj, obj.GetType(), mapper);
            MapUiObjectsFromFields(obj, obj.GetType(), mapper);
        }

        private static void MapUiObjectsFromProperties(object obj, Type type, IAndroidUiMapper mapper)
        {
            if (type.BaseType != null)
            {
                MapUiObjectsFromProperties(obj, type.BaseType, mapper);
            }

            var properties = type.GetProperties(BindingFlags.Instance |
                                                BindingFlags.NonPublic |
                                                BindingFlags.Public);

            foreach (var property in properties)
            {
                if (property.PropertyType != typeof(UiObject) && property.PropertyType != typeof(UiObjects))
                {
                    continue;
                }

                var attributes = property.GetCustomAttributes(typeof(UiMapAttribute), true);
                if (attributes.Length >= 1)
                {
                    var withs = attributes.Cast<UiMapAttribute>().Select(u => u.With).ToArray();
                    if (property.PropertyType == typeof(UiObject))
                    {
                        property.SetValue(obj, mapper.MapUiNode(withs));
                    }
                    else
                    {
                        property.SetValue(obj, mapper.MapUiNodes(withs));
                    }
                }
            }
        }

        private static void MapUiObjectsFromFields(object obj, Type type, IAndroidUiMapper mapper)
        {
            if (type.BaseType != null)
            {
                MapUiObjectsFromFields(obj, type.BaseType, mapper);
            }

            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.FieldType != typeof(UiObject) && field.FieldType != typeof(UiObjects))
                {
                    continue;
                }

                var attributes = field.GetCustomAttributes(typeof(UiMapAttribute), true);
                if (attributes.Length >= 1)
                {
                    var withs = attributes.Cast<UiMapAttribute>().Select(u => u.With).ToArray();
                    if (field.FieldType == typeof(UiObject))
                    {
                        field.SetValue(obj, mapper.MapUiNode(withs));
                    }
                    else
                    {
                        field.SetValue(obj, mapper.MapUiNodes(withs));
                    }
                }
            }
        }
    }
}
