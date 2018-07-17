using System.Reflection;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Ui.Objects;
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
            [MapUiObject(Text = "test")]
            private UiObject _fieldObject;

            [MapUiObject(Class = "hej")]
            public UiObject PropertyObject { get; private set; }

            [MapUiObject(Index = "0", Class = "class")]
            public UiObject PropertyObjectWithMultiple { get; private set; }

            [MapUiObject(Class = "test")]
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
            Assert.AreEqual(1, _exampleClass.PropertyObject.Wheres.Count);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithInternalPropertiesThatUseAttribute_ShouldInitializeUiObject()
        {
            Assert.IsNotNull(_exampleClass.InternalProperty);
            Assert.AreEqual(1, _exampleClass.InternalProperty.Wheres.Count);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithFieldsThatUseAttribute_ShouldInitializeUiObject()
        {
            var field = (UiObject)typeof(ExampleClass).GetField("_fieldObject", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_exampleClass);
            Assert.IsNotNull(field);
            Assert.AreEqual(1, field.Wheres.Count);
        }


        [Test]
        public void InitializeUiObjects_WhenHavingClassWithPropertyThatUseMultipleAttributes_ShouldInitializeUiObjects()
        {
            Assert.IsNotNull(_exampleClass.PropertyObjectWithMultiple);
            Assert.AreEqual(2, _exampleClass.PropertyObjectWithMultiple.Wheres.Count);
        }
    }
}
