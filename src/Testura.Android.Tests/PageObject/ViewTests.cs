using System.Reflection;
using Moq;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;
using Testura.Android.PageObject;
using Testura.Android.PageObject.Attributes;
using Testura.Android.Util;
#pragma warning disable 169

namespace Testura.Android.Tests.PageObject
{
    [TestFixture]
    public class ViewTests
    {
        private ExampleClass _exampleClass;

        private class ExampleClass
        {
            [UiMap(with: AttributeTags.Text, value: "test")]
            private UiObject _fieldObject;

            [UiMap(with: AttributeTags.Text, value: "test")]
            private UiObjects _fieldObjects;

            [UiMap(with: AttributeTags.Class, value: "test")]
            public UiObject PropertyObject { get; private set; }

            [UiMap(with: AttributeTags.Index, value: "0")]
            [UiMap(with: AttributeTags.Class, value: "test")]
            public UiObject PropertyObjectWithMultiple { get; private set; }

            [UiMap(with: AttributeTags.Class, value: "test")]
            public UiObjects PropertyObjects { get; private set; }

            [UiMap(with: AttributeTags.Class, value: "test")]
            internal UiObject InternalProperty { get; private set; }
        }

        [SetUp]
        public void SetUp()
        {
            _exampleClass = new ExampleClass();
            ViewFactory.MapUiNodes(new AndroidDevice(new DeviceConfiguration { Dependencies = DependencyHandling.NeverInstall }), _exampleClass);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithPropertiesThatUseAttribute_ShouldInitializeUiObject()
        {
            Assert.IsNotNull(_exampleClass.PropertyObject);
            Assert.AreEqual(1, _exampleClass.PropertyObject.Withs.Count);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithInternalPropertiesThatUseAttribute_ShouldInitializeUiObject()
        {
            Assert.IsNotNull(_exampleClass.InternalProperty);
            Assert.AreEqual(1, _exampleClass.InternalProperty.Withs.Count);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithFieldsThatUseAttribute_ShouldInitializeUiObject()
        {
            var field = (UiObject)typeof(ExampleClass).GetField("_fieldObject", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_exampleClass);
            Assert.IsNotNull(field);
            Assert.AreEqual(1, field.Withs.Count);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithPropertiesThatUseAttribute_ShouldInitializeUiObjects()
        {
            Assert.IsNotNull(_exampleClass.PropertyObjects);
            Assert.AreEqual(1, _exampleClass.PropertyObjects.Withs.Count);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithFieldsThatUseAttribute_ShouldInitializeUiObjects()
        {
            var fields = (UiObjects)typeof(ExampleClass).GetField("_fieldObjects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_exampleClass);
            Assert.IsNotNull(fields);
            Assert.AreEqual(1, fields.Withs.Count);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithPropertyThatUseMultipleAttributes_ShouldInitializeUiObjects()
        {
            Assert.IsNotNull(_exampleClass.PropertyObjectWithMultiple);
            Assert.AreEqual(2, _exampleClass.PropertyObjectWithMultiple.Withs.Count);
        }
    }
}
