using Moq;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Objects;
using Testura.Android.Device.Ui.Search;
using Testura.Android.PageObject;

namespace Testura.Android.Tests.PageObject
{
    [TestFixture]
    public class ViewTests
    {
        private class ExampleClass : View
        {
            private UiObject _fieldObject;

            public ExampleClass(IAndroidDevice device) : base(device)
            {
            }

            [InitializeUiObject((h) => h++)]
            public UiObject PropertyObject { get; set; }
        }

        [Test]
        public void InitializeUiObjects_WhenHavingClassWithFieldAndPropertiesThatUseAttribute_ShouldBeInitialized()
        {
            var example = new ExampleClass(new Mock<IAndroidDevice>().Object);

            Assert.IsNotNull(example.PropertyObject);
        }
    }

    
}
