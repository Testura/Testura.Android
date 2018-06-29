using System;
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
            var uiObjectOne = _testHelper.CreateUiObject(By.Class("test"), 0);
            var uiObjectTwo = _testHelper.CreateUiObject(By.ResourceId("test"), 2000);
            var foundObject = UiWait.ForAny(new Func<TimeSpan, bool>[] { uiObjectOne.IsVisible, uiObjectTwo.IsHidden }, TimeSpan.FromSeconds(10));
            Assert.AreEqual(uiObjectOne, foundObject);
        }

        [Test]
        public void ForAny_IfNoObjectIsFound_ShouldThrowException()
        {
            var uiObject = _testHelper.CreateUiObject(By.Class("hej"), 0, true);
            Assert.Throws<UiNodeNotFoundException>(() => UiWait.ForAny(new Func<TimeSpan, bool>[] { uiObject.IsVisible }, TimeSpan.FromSeconds(10)));
        }

        [Test]
        public void ForAny_IfObjectLaterThrowsException_ShouldNotCrash()
        {
            var uiObjectOne = _testHelper.CreateUiObject(By.Class("test"), 0);
            var uiObjectTwo = _testHelper.CreateUiObject(By.ResourceId("test"), 500, true);
            var foundObject = UiWait.ForAny(new Func<TimeSpan, bool>[] { uiObjectOne.IsVisible, uiObjectTwo.IsHidden}, TimeSpan.FromSeconds(10));
            Assert.AreEqual(uiObjectOne, foundObject);
            Thread.Sleep(1000);
        }

        [Test]
        public void ForAll_WhenFindAll_ShouldNotThrowException()
        {
            var uiObjectOne = _testHelper.CreateUiObject(By.Class("test"), 500);
            var uiObjectTwo = _testHelper.CreateUiObject(By.ResourceId("test"), 500);
            UiWait.ForAll(new Func<TimeSpan, bool>[] { uiObjectOne.IsVisible, uiObjectTwo.IsVisible }, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void ForAll_WhenCantFindAll_ShouldThrowException()
        {
            var uiObjectOne = _testHelper.CreateUiObject(By.Class("test"), 500, true);
            var uiObjectTwo = _testHelper.CreateUiObject(By.ResourceId("test"), 500, true);
            Assert.Throws<UiNodeNotFoundException>(() => UiWait.ForAll(new Func<TimeSpan, bool>[] { uiObjectOne.IsVisible, uiObjectTwo.IsVisible }, TimeSpan.FromSeconds(1)));
        }
    }
}
