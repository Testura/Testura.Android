using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Default;
using Testura.Android.Device.UiAutomator.Server;
using Testura.Android.Device.UiAutomator.Ui;
using Testura.Android.Device.UiAutomator.Ui.Search;
using Testura.Android.Device.UiAutomator.Ui.Util;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Helpers;

namespace Testura.Android.Tests.Util.Helpers
{
    [TestFixture]
    public class UiWaitTests
    {
        [Test]
        public void ForAny_WhenFindNodeOnFirst_ShouldReturnFirst()
        {
            var uiObject = CreateUiObject();
            var methodOne = new Func<int, BaseUiObject>(i => uiObject);
            var methodTwo = new Func<int, BaseUiObject>(i =>
            {
                Thread.Sleep(1000);
                return null;
            });
            var foundObject = UiWait.ForAny(10, methodOne, methodTwo);
            Assert.AreEqual(uiObject, foundObject);
        }

        [Test]
        public void ForAny_IfNoObjectIsFound_ShouldThrowException()
        {
            var methodOne = new Func<int, BaseUiObject>(i => null);
            var methodTwo = new Func<int, BaseUiObject>(i => null);
            Assert.Throws<UiNodeNotFoundException>(() => UiWait.ForAny(10, methodOne, methodTwo));
        }

        [Test]
        public void ForAny_IfObjectLaterThrowsException_ShouldNotCrash()
        {
            var uiObject = CreateUiObject();
            var methodOne = new Func<int, BaseUiObject>(i => uiObject);
            var methodTwo = new Func<int, BaseUiObject>(i =>
            {
                Thread.Sleep(250);
                throw new Exception("Could not find");
            });
            var foundObject = UiWait.ForAny(10, methodOne, methodTwo);
            Assert.AreEqual(uiObject, foundObject);
            //Verify that we don't get a crash
            Thread.Sleep(1000);
        }

        private UiObject CreateUiObject()
        {
            var uiService = new UiService(null, null, null);
            uiService.InitializeServiceOwner(new Mock<IAndroidDevice>().Object);
            return uiService.CreateUiObject(With.Class("class"));
        }
    }
}
