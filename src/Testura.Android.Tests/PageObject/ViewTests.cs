using System.Reflection;
using Moq;
using NUnit.Framework;
using Testura.Android.Device;
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

        private class ExampleClass : View
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

        [SetUp]
        public void SetUp()
        {
            _testHelper = new TestHelper();
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithPropertiesThatUseAttribute_ShouldInitializeUiObject()
        {
            var uiObject = _testHelper.CreateUiObject(With.Text("test"), 0);
            _testHelper.UiServiceMock.Setup(u => u.CreateUiObject(It.IsAny<With>())).Returns(uiObject);
            var example = new ExampleClass(_testHelper.DeviceMock.Object);
            Assert.IsNotNull(example.PropertyObject);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithFieldsThatUseAttribute_ShouldInitializeUiObject()
        {
            var uiObject = _testHelper.CreateUiObject(With.Text("test"), 0);
            _testHelper.UiServiceMock.Setup(u => u.CreateUiObject(It.IsAny<With>())).Returns(uiObject);
            var example = new ExampleClass(_testHelper.DeviceMock.Object);
            var field = typeof(ExampleClass).GetField("_fieldObject", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(example);
            Assert.IsNotNull(field);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithPropertiesThatUseAttribute_ShouldInitializeUiObjects()
        {
            var uiObject = _testHelper.CreateUiObjects(With.Text("test"), 0);
            _testHelper.UiServiceMock.Setup(u => u.CreateUiObjects(It.IsAny<With>())).Returns(uiObject);
            var example = new ExampleClass(_testHelper.DeviceMock.Object);
            Assert.IsNotNull(example.PropertyObjects);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithFieldsThatUseAttribute_ShouldInitializeUiObjects()
        {
            var uiObject = _testHelper.CreateUiObjects(With.Text("test"), 0);
            _testHelper.UiServiceMock.Setup(u => u.CreateUiObjects(It.IsAny<With>())).Returns(uiObject);
            var example = new ExampleClass(_testHelper.DeviceMock.Object);
            var fields = typeof(ExampleClass).GetField("_fieldObjects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(example);
            Assert.IsNotNull(fields);
        }
    }

    
}
