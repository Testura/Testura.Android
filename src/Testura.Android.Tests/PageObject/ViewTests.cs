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
        private TestHelper _testHelper;

        private class ExampleClass : Hmm
        {
            [Create(with: AttributeTags.Text, value: "test")]
            private UiObject _fieldObject;

            [Create(with: AttributeTags.Text, value: "test")]
            private UiObjects _fieldObjects;

            public ExampleClass(IAndroidDevice device) 
                : base(device)
            {
            }

            [Create(with: AttributeTags.Class, value: "test")]
            public UiObject PropertyObject { get; private set; }

            [Create(with: AttributeTags.Class, value: "test")]
            public UiObjects PropertyObjects { get; private set; }
        }

        private class Hmm : View
        {
            [Create(with: AttributeTags.Text, value: "test")]
            private UiObject _childFieldObject;

            protected Hmm(IAndroidDevice device) : base(device)
            {
            }

            [Create(with: AttributeTags.Class, value: "test")]
            protected UiObject ChildObjectProp { get; set; }

            [Create(with: AttributeTags.Class, value: "test")]
            protected UiObject ChildObjectPropWithPrivateSet { get; private set; }
        }

        [SetUp]
        public void SetUp()
        {
            _testHelper = new TestHelper();
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithPropertiesThatUseAttribute_ShouldInitializeUiObject()
        {
            var example = new ExampleClass(new AndroidDevice(new DeviceConfiguration { Dependencies = DependencyHandling.NeverInstall}));
            Assert.IsNotNull(example.PropertyObject);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithFieldsThatUseAttribute_ShouldInitializeUiObject()
        {
            var example = new ExampleClass(new AndroidDevice(new DeviceConfiguration { Dependencies = DependencyHandling.NeverInstall }));
            var field = typeof(ExampleClass).GetField("_fieldObject", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(example);
            Assert.IsNotNull(field);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithPropertiesThatUseAttribute_ShouldInitializeUiObjects()
        {
            var example = new ExampleClass(new AndroidDevice(new DeviceConfiguration { Dependencies = DependencyHandling.NeverInstall }));
            Assert.IsNotNull(example.PropertyObjects);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithFieldsThatUseAttribute_ShouldInitializeUiObjects()
        {
            var example = new ExampleClass(new AndroidDevice(new DeviceConfiguration { Dependencies = DependencyHandling.NeverInstall }));
            var fields = typeof(ExampleClass).GetField("_fieldObjects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(example);
            Assert.IsNotNull(fields);
        }

        [Test]
        public void InitializeUiObjects_WhenInheritFromBaseClassThatInheirtFromViewAndHaveProperties_ShouldInitializeUiObjectsOnInheritedView()
        {
            var example = new ExampleClass(new AndroidDevice(new DeviceConfiguration { Dependencies = DependencyHandling.NeverInstall }));
            var property = typeof(Hmm).GetProperty("ChildObjectProp", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(example);
            Assert.IsNotNull(property);
        }

        [Test]
        public void InitializeUiObjects_WhenInheritFromBaseClassThatInheirtFromViewAndHaveFields_ShouldInitializeUiObjectsOnInheritedView()
        {
            var example = new ExampleClass(new AndroidDevice(new DeviceConfiguration { Dependencies = DependencyHandling.NeverInstall }));
            var fields = typeof(Hmm).GetField("_childFieldObject", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(example);
            Assert.IsNotNull(fields);
        }
    }

    
}
