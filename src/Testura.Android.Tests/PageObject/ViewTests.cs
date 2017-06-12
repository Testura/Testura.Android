using System.Reflection;
using Moq;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;
using Testura.Android.PageObject;
using Testura.Android.PageObject.Attributes;
using Testura.Android.Util;

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

            public ExampleClass(IAndroidDevice device) 
                : base(device)
            {
            }

            [Create(with: AttributeTags.Class, value: "test")]
            public UiObject PropertyObject { get; private set; }
        }

        [SetUp]
        public void SetUp()
        {
            _testHelper = new TestHelper();
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithPropertiesThatUseAttribute_ShouldBeInitialized()
        {
            var uiObject = _testHelper.CreateUiObject(With.Text("test"), 0);
            _testHelper.UiServiceMock.Setup(u => u.CreateUiObject(It.IsAny<With>())).Returns(uiObject);
            var example = new ExampleClass(_testHelper.DeviceMock.Object);
            Assert.IsNotNull(example.PropertyObject);
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithFieldsThatUseAttribute_ShouldBeInitialized()
        {
            var uiObject = _testHelper.CreateUiObject(With.Text("test"), 0);
            _testHelper.UiServiceMock.Setup(u => u.CreateUiObject(It.IsAny<With>())).Returns(uiObject);
            var example = new ExampleClass(_testHelper.DeviceMock.Object);
            var field = typeof(ExampleClass).GetField("_fieldObject", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(example);
            Assert.IsNotNull(field);
        }
    }

    
}
