using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Helpers;

namespace Testura.Android.Tests.Util.Helpers
{
    [TestFixture]
    public class UiWaitTests
    {
        private TestHelper _testHelper;

        [SetUp]
        public void SetUp()
        {
            _testHelper = new TestHelper();
        }

        [Test]
        public void ForAny_WhenFindNodeOnFirst_ShouldReturnFirst()
        {
            var uiObjectOne = _testHelper.CreateUiObject(With.Class("test"), 0);
            var uiObjectTwo = _testHelper.CreateUiObject(With.ResourceId("test"), 2000);
            var foundObject = UiWait.ForAny(10, uiObjectOne.IsVisible, uiObjectTwo.IsHidden);
            Assert.AreEqual(uiObjectOne, foundObject);
        }

        [Test]
        public void ForAny_IfNoObjectIsFound_ShouldThrowException()
        {
            var uiObject = _testHelper.CreateUiObject(With.Class("hej"), 0, true);
            Assert.Throws<UiNodeNotFoundException>(() => UiWait.ForAny(10, uiObject.IsVisible));
        }

        [Test]
        public void ForAny_IfObjectLaterThrowsException_ShouldNotCrash()
        {
            var uiObjectOne = _testHelper.CreateUiObject(With.Class("test"), 0);
            var uiObjectTwo = _testHelper.CreateUiObject(With.ResourceId("test"), 500, true);
            var foundObject = UiWait.ForAny(10, uiObjectOne.IsVisible, uiObjectTwo.IsHidden);
            Assert.AreEqual(uiObjectOne, foundObject);
            Thread.Sleep(1000);
        }

        [Test]
        public void ForAll_WhenFindAll_ShouldNotThrowException()
        {
            var uiObjectOne = _testHelper.CreateUiObject(With.Class("test"), 500);
            var uiObjectTwo = _testHelper.CreateUiObject(With.ResourceId("test"), 500);
            UiWait.ForAll(1, uiObjectOne.IsVisible, uiObjectTwo.IsVisible);
        }

        [Test]
        public void ForAll_WhenCantFindAll_ShouldThrowException()
        {
            var uiObjectOne = _testHelper.CreateUiObject(With.Class("test"), 500, true);
            var uiObjectTwo = _testHelper.CreateUiObject(With.ResourceId("test"), 500, true);
            Assert.Throws<UiNodeNotFoundException>(() => UiWait.ForAll(1, uiObjectOne.IsVisible, uiObjectTwo.IsVisible));
        }
    }
}
